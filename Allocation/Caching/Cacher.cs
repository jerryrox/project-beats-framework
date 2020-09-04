using System;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Threading.Futures;

namespace PBFramework.Allocation.Caching
{
    public abstract class Cacher<TKey, TValue> : ICacher<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// Table for all pending requests.
        /// </summary>
        private Dictionary<TKey, CacheRequest<TValue>> requests = new Dictionary<TKey, CacheRequest<TValue>>();

        /// <summary>
        /// Table for all cached data.
        /// </summary>
        private Dictionary<TKey, CachedData<TValue>> caches = new Dictionary<TKey, CachedData<TValue>>();


        public uint Request(TKey key, TaskListener<TValue> listener)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            // If there is a cached resource, return that straight away.
            if (caches.TryGetValue(key, out CachedData<TValue> cached))
            {
                AcquireData(listener, cached);
                return 0;
            }
            // If there is already an on-going request, hook listener on to that request.
            if (requests.TryGetValue(key, out CacheRequest<TValue> request))
            {
                return request.Listen(listener);
            }
            // Else, start a new request and listen to it.
            request = RequestNew(key);
            return request.Listen(listener);
        }

        public void Remove(TKey key, uint id)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            // Force stop request and remove it.
            if (requests.TryGetValue(key, out CacheRequest<TValue> request))
            {
                RemoveRequest(id, key, request);
                return;
            }
            // Destroy cached resource and remove it.
            if (caches.TryGetValue(key, out CachedData<TValue> data))
            {
                RemoveData(key, data);
                return;
            }
        }

        public void RemoveDelayed(TKey key, uint id, float delay = 2f)
        {
			if(key == null) throw new ArgumentNullException(nameof(key));

            var timer = CreateTimer();
            timer.Limit = delay;
            timer.IsCompleted.OnNewValue += (completed) => {
                if(completed)
                    Remove(key, id);
            };
            timer.Start();
        }

        public bool IsCached(TKey key) => caches.ContainsKey(key);

        /// <summary>
        /// Creates a new future which represents the requesting process.
        /// </summary>
        protected abstract IControlledFuture<TValue> CreateRequest(TKey key);

        /// <summary>
        /// Creates a new timer instance to use for delayed destruction.
        /// </summary>
        protected virtual ITimer CreateTimer() => new SynchronizedTimer();

        /// <summary>
        /// Destroys the resource so it becomes unusable.
        /// </summary>
        protected virtual void DestroyData(TValue data) {}

        /// <summary>
        /// Initializes a new CacheRequest for specified key.
        /// </summary>
        private CacheRequest<TValue> RequestNew(TKey key)
        {
            // Create request
            var request = CreateRequest(key);
            var cacheRequest = new CacheRequest<TValue>(request);

            // Set default event handling.
            request.Output.OnNewValue += (value) =>
            {
                OnResourceLoaded(cacheRequest, key, value);
            };

            // Add to requests list and start.
            requests.Add(key, cacheRequest);
			request.Start();
            return cacheRequest;
        }

        /// <summary>
        /// Makes the specified listener acquire the data.
        /// </summary>
        private void AcquireData(TaskListener<TValue> listener, CachedData<TValue> cached)
        {
            cached.Lock++;
            listener.SetFinished(cached.Value);
        }

        /// <summary>
        /// Removes the specified request.
        /// </summary>
        private void RemoveRequest(uint id, TKey key, CacheRequest<TValue> request)
        {
            // Unhook the callback.
            request.Remove(id);

            // If no more reference remaining on the resource, then revoke the request.
            if(request.ListenerCount <= 0)
            {
                request.Request.Dispose();
                requests.Remove(key);
            }
        }

        /// <summary>
        /// Removes a lock on the specified data and if necessary, complete remove the entry from table.
        /// </summary>
        private void RemoveData(TKey key, CachedData<TValue> data)
        {
            // Remove reference count on the resource.
            data.Lock --;

            // If no more reference remaining on the resource, just destroy it.
            if(data.Lock <= 0)
            {
                DestroyData(data.Value);
                this.caches.Remove(key);
            }
        }

        /// <summary>
		/// Callback from a request when it has finished loading its resources.
		/// </summary>
		private void OnResourceLoaded(CacheRequest<TValue> request, TKey identifier, TValue value)
		{
			// Remove request from requests table.
			requests.Remove(identifier);

			// Cache resource.
			if(!caches.ContainsKey(identifier))
			{
				caches.Add(identifier, new CachedData<TValue>() {
					Value = value,
					Lock = request.ListenerCount
				});
			}
		}
    }

    public abstract class Cacher<TValue> : Cacher<string, TValue>, ICacher<TValue>
        where TValue : class
    {

    }
}