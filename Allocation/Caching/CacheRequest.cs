using System;
using System.Collections.Generic;
using PBFramework.Threading.Futures;

namespace PBFramework.Allocation.Caching
{
    public class CacheRequest<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// The key associated with this request.
        /// </summary>
        public object Key { get; private set; }

        /// <summary>
        /// The requesting instance.
        /// </summary>
        public IFuture<T> Request { get; private set; }

        /// <summary>
        /// The list of listeners waiting for the resource request to be completed.
        /// </summary>
        public List<CacheListener<T>> Listeners { get; } = new List<CacheListener<T>>();

        /// <summary>
        /// The actual value loaded from the inner request.
        /// </summary>
        public T Value => Request.Output.Value;

        /// <summary>
        /// Returns whether the inner resource load request is completed.
        /// </summary>
        public bool IsComplete => Request.IsCompleted.Value;


        public CacheRequest(object key, IFuture<T> request)
        {
            this.Key = key;
            this.Request = request;
        }

        /// <summary>
        /// Creates a new listener that listens to the completion of this request.
        /// </summary>
        public CacheListener<T> Listen()
        {
            AssertNotDisposed();

            var listener = (
                IsComplete ?
                new CacheListener<T>(Key, Value) :
                new CacheListener<T>(Key, Request)
            );
            Listeners.Add(listener);
            return listener;
        }

        /// <summary>
        /// Attempts to remove and dispose the specified listener, if managed by this request.
        /// </summary>
        public bool Unlisten(CacheListener<T> listener)
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
                throw new ObjectDisposedException(nameof(CacheRequest<T>));
        }
    }
}