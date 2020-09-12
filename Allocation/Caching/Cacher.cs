using System;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    public abstract class Cacher<TKey, TValue> : ICacher<TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// Table for all pending requests.
        /// </summary>
        private Dictionary<object, CacheRequest<TValue>> requests = new Dictionary<object, CacheRequest<TValue>>();


        public CacheListener<TValue> Request(TKey key)
        {
            object convertedKey = ConvertKey(key);

            if (convertedKey == null)
                throw new ArgumentNullException(nameof(convertedKey));

            // If there is a cached resource, return that straight away.
            if(requests.TryGetValue(convertedKey, out CacheRequest<TValue> request))
                return request.Listen();

            // Else, start a new request and listen to it.
            request = RequestNew(key, convertedKey);
            return request.Listen();
        }

        public void Remove(CacheListener<TValue> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            if (requests.TryGetValue(listener.Key, out CacheRequest<TValue> request))
            {
                RemoveListener(listener, request);
                return;
            }
        }

        public void RemoveDelayed(CacheListener<TValue> listener, float delay = 2f)
        {
			if(listener == null)
                throw new ArgumentNullException(nameof(listener));

            var timer = CreateTimer();
            timer.Limit = delay;
            timer.OnFinished += () => Remove(listener);
            timer.Start();
        }

        public bool IsCached(TKey key)
        {
            if(requests.TryGetValue(ConvertKey(key), out CacheRequest<TValue> value))
                return value.IsComplete;
            return false;
        }

        /// <summary>
        /// Returns a new task that retrieves the data associated with the key.
        /// </summary>
        protected abstract ITask<TValue> CreateRequest(TKey key);

        /// <summary>
        /// Creates a new timer instance to use for delayed destruction.
        /// </summary>
        protected virtual ITimer CreateTimer() => new SynchronizedTimer();

        /// <summary>
        /// Destroys the resource so it becomes unusable.
        /// </summary>
        protected virtual void DestroyData(TValue data) {}

        /// <summary>
        /// Converts the specified key to the desired format.
        /// This will be useful when different instances of TKey should be considered the same key
        /// as part of the app-specific requirements.
        /// </summary>
        protected virtual object ConvertKey(TKey key) => key;

        /// <summary>
        /// Initializes a new CacheRequest for specified key.
        /// </summary>
        private CacheRequest<TValue> RequestNew(TKey key, object convertedKey)
        {
            // Create request
            var request = CreateRequest(key);
            var cacheRequest = new CacheRequest<TValue>(convertedKey, request);

            // Add to requests list and start.
            requests.Add(convertedKey, cacheRequest);
            cacheRequest.StartRequest();
            return cacheRequest;
        }

        /// <summary>
        /// Removes the specified listener from the request.
        /// </summary>
        private void RemoveListener(CacheListener<TValue> listener, CacheRequest<TValue> request)
        {
            // Remove listener
            request.Unlisten(listener);

            // If no more reference remaining on the resource, then revoke the request.
            if(request.Listeners.Count <= 0)
            {
                if(request.IsComplete)
                    DestroyData(request.Value);
                request.Dispose();
                requests.Remove(request.Key);
            }
        }
    }
}