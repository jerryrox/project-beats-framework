using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Threading.Futures;

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
        private IFuture<T> request;


        /// <summary>
        /// The requesting instance.
        /// </summary>
        public IFuture<T> Request => request;

        /// <summary>
        /// Returns the number of listeners waiting for the request to finish.
        /// </summary>
        public int ListenerCount => listeners.Count;


        public CacheRequest(IFuture<T> request)
        {
            this.request = request;

            // Add callback handler action.
            request.Progress.OnNewValue += (p) =>
            {
                foreach (var listener in listeners.Values)
                {
                    if (listener != null)
                    {
                        listener.Report(p);
                    }
                }
            };
            request.IsCompleted.OnNewValue += (completed) =>
            {
                if(!completed)
                    return;
                    
                T output = request.Output.Value;
                foreach(var listener in listeners.Values)
                {
                    if (listener != null)
                    {
                        listener.Value = output;
                        listener.InvokeFinished(output);
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