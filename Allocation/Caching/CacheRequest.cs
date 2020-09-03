using System;
using System.Collections.Generic;
using PBFramework.Threading.Futures;

namespace PBFramework.Allocation.Caching
{
    public class CacheRequest<TKey, TValue> : IDisposable
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// The key associated with this request.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// The requesting instance.
        /// </summary>
        public IFuture<TValue> Request { get; private set; }

        /// <summary>
        /// The list of listeners waiting for the resource request to be completed.
        /// </summary>
        public List<CacheListener<TKey, TValue>> Listeners { get; } = new List<CacheListener<TKey, TValue>>();

        /// <summary>
        /// The actual value loaded from the inner request.
        /// </summary>
        public TValue Value => Request.Output.Value;

        /// <summary>
        /// Returns whether the inner resource load request is completed.
        /// </summary>
        public bool IsComplete => Request.IsCompleted.Value;


        public CacheRequest(TKey key, IFuture<TValue> request)
        {
            this.Request = request;
        }

        /// <summary>
        /// Creates a new listener that listens to the completion of this request.
        /// </summary>
        public CacheListener<TKey, TValue> Listen()
        {
            AssertNotDisposed();

            var listener = (
                IsComplete ?
                new CacheListener<TKey, TValue>(Key, Value) :
                new CacheListener<TKey, TValue>(Key, Request)
            );
            Listeners.Add(listener);
            return listener;
        }

        /// <summary>
        /// Attempts to remove and dispose the specified listener, if managed by this request.
        /// </summary>
        public bool Unlisten(CacheListener<TKey, TValue> listener)
        {
            AssertNotDisposed();

            if (Listeners.Remove(listener))
            {
                listener.Dispose();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            foreach(var listener in Listeners)
                listener.Dispose();
            Listeners.Clear();

            Request.Dispose();
            Request = null;
        }

        /// <summary>
        /// Asserts that this object hasn't been disposed.
        /// </summary>
        private void AssertNotDisposed()
        {
            if(Request == null)
                throw new ObjectDisposedException(nameof(CacheRequest<TKey, TValue>));
        }
    }
}