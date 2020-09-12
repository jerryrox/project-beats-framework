using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    public class CacheRequest<T> : IDisposable {

        /// <summary>
        /// The key associated with this request.
        /// </summary>
        public object Key { get; private set; }

        /// <summary>
        /// The list of listeners waiting for the resource request to be completed.
        /// </summary>
        public List<CacheListener<T>> Listeners { get; } = new List<CacheListener<T>>();

        /// <summary>
        /// The listener which listens to the requester's events.
        /// </summary>
        public TaskListener<T> RequestListener { get; } = new TaskListener<T>();

        /// <summary>
        /// The requester that retrieves the actual data.
        /// </summary>
        public ITask<T> Request { get; private set; }

        /// <summary>
        /// The actual value loaded from the inner request.
        /// </summary>
        public T Value => RequestListener.Value;

        /// <summary>
        /// Returns whether the inner resource load request is completed.
        /// </summary>
        public bool IsComplete => RequestListener.IsFinished;


        public CacheRequest(object key, ITask<T> request)
        {
            this.Key = key;
            this.Request = request;
            
            // // Add callback handler action.
            RequestListener.OnProgress += (p) =>
            {
                foreach (var listener in Listeners)
                {
                    if (listener != null)
                        listener.SetProgress(p);
                }
            };
            RequestListener.OnFinished += (data) =>
            {
                foreach(var listener in Listeners)
                {
                    if (listener != null)
                        listener.SetFinished(data);
                }
            };
        }

        /// <summary>
        /// Creates a new listener that listens to the completion of this request.
        /// </summary>
        public CacheListener<T> Listen()
        {
            AssertNotDisposed();

            var listener = new CacheListener<T>(Key);
            if(IsComplete)
                listener.SetFinished(Value);
                
            Listeners.Add(listener);
            return listener;
        }

        /// <summary>
        /// Attempts to remove the specified listener, if managed by this request.
        /// </summary>
        public bool Unlisten(CacheListener<T> listener)
        {
            AssertNotDisposed();

            return Listeners.Remove(listener);
        }

        /// <summary>
        /// Starts requesting on the actual requester.
        /// </summary>
        public void StartRequest()
        {
            Request.StartTask(RequestListener);
        }

        public void Dispose()
        {
            Listeners.Clear();
            Request.RevokeTask(true);
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