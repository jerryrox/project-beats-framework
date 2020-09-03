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
        private Dictionary<TKey, CacheRequest<TKey, TValue>> requests = new Dictionary<TKey, CacheRequest<TKey, TValue>>();


        public CacheListener<TKey, TValue> Request(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            // If there is already an on-going/completed request, hook listener on to that request.
            if (requests.TryGetValue(key, out CacheRequest<TKey, TValue> request))
                return request.Listen();

            // Else, start a new request and listen to it.
            request = RequestNew(key);
            return request.Listen();
        }

        public void Remove(CacheListener<TKey, TValue> listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            if (requests.TryGetValue(listener.Key, out CacheRequest<TKey, TValue> request))
            {
                RemoveListener(listener, request);
                return;
            }
        }

        public void RemoveDelayed(CacheListener<TKey, TValue> listener, float delay = 2f)
        {
			if(listener == null) throw new ArgumentNullException(nameof(listener));

            var timer = CreateTimer();
            timer.Limit = delay;
            timer.IsCompleted.OnNewValue += (completed) =>
            {
                if(completed)
                    Remove(listener);
            };
            timer.Start();
        }

        public bool IsCached(TKey key)
        {
            if(requests.TryGetValue(key, out CacheRequest<TKey, TValue> value))
                return value.IsComplete;
            return false;
        }

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
        private CacheRequest<TKey, TValue> RequestNew(TKey key)
        {
            // Create request
            var request = CreateRequest(key);
            var cacheRequest = new CacheRequest<TKey, TValue>(key, request);

            // Add to requests list and start.
            requests.Add(key, cacheRequest);
			request.Start();
            return cacheRequest;
        }

        /// <summary>
        /// Removes the specified listener from the request.
        /// </summary>
        private void RemoveListener(CacheListener<TKey, TValue> listener, CacheRequest<TKey, TValue> request)
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

    public abstract class Cacher<TValue> : Cacher<string, TValue>, ICacher<TValue>
        where TValue : class
    {

    }
}