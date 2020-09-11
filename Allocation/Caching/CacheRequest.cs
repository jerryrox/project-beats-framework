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
        private Dictionary<uint, TaskListener<T>> listeners = new Dictionary<uint, TaskListener<T>>();

        /// <summary>
        /// The next hook id number to be assigned to a new callback.
        /// </summary>
        private uint nextId;


        /// <summary>
        /// The listener which listens to the requester's events.
        /// </summary>
        public TaskListener<T> RequestListener { get; } = new TaskListener<T>();

        /// <summary>
        /// The requester that retrieves the actual data.
        /// </summary>
        public ITask<T> Request { get; private set; }

        /// <summary>
        /// Returns the number of listeners waiting for the request to finish.
        /// </summary>
        public int ListenerCount => listeners.Count;


        public CacheRequest(ITask<T> request)
        {
            this.Request = request;
            
            // Add callback handler action.
            RequestListener.OnProgress += (p) =>
            {
                foreach (var listener in listeners.Values)
                {
                    if (listener != null)
                        listener.SetProgress(p);
                }
            };
            RequestListener.OnFinished += (data) =>
            {
                foreach(var listener in listeners.Values)
                {
                    if (listener != null)
                        listener.SetFinished(data);
                }
            };
        }

        public uint Listen(TaskListener<T> listener)
        {
            // Increment id
            nextId ++;
            // Register callback.
            listeners.Add(nextId, listener);
            // Return id for referencing.
            return nextId;
        }

        public void Remove(uint id) => listeners.Remove(id);
    }
}