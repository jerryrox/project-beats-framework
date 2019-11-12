using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    public class CacheRequest<T> {

        /// <summary>
        /// Table of listeners waiting for the request to finish.
        /// </summary>
        private Dictionary<uint, IReturnableProgress<T>> listeners = new Dictionary<uint, IReturnableProgress<T>>();

        /// <summary>
        /// The next hook id number to be assigned to a new callback.
        /// </summary>
        private uint nextId;

        /// <summary>
        /// The actual requester object.
        /// </summary>
        private IPromise<T> request;


        /// <summary>
        /// The requesting promise.
        /// </summary>
        public IPromise<T> Request => request;

        /// <summary>
        /// Returns the number of listeners waiting for the request to finish.
        /// </summary>
        public int ListenerCount => listeners.Count;


        public CacheRequest(IPromise<T> request)
        {
            this.request = request;

            // Add callback handler action.
            request.OnFinishedResult += (v) =>
            {
                foreach(var listener in listeners.Values)
                {
                    if (listener != null)
                    {
                        listener.Value = v;
                        listener.InvokeFinished(v);
                    }
                }
            };
        }

        public uint Listen(IReturnableProgress<T> progress)
        {
            // Increment id
            nextId ++;
            // Register callback.
            listeners.Add(nextId, progress);
            // Return id for referencing.
            return nextId;
        }

        public void Remove(uint id) => listeners.Remove(id);
    }
}