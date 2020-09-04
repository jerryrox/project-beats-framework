using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Class which encapsulates resource loading, destruction,
    /// and management of data and load ID away from consumer for cleaner code.
    /// </summary>
    public class CacherAgent<TKey, TValue> : TaskListener<TValue>
        where TKey : class
        where TValue : class
    {
        private ICacher<TKey, TValue> cacher;

        private TKey lastKey;
        private uint lastId;


        /// <summary>
        /// Whether removing data should be done through delayed removal.
        /// </summary>
        public bool UseDelayedRemove { get; set; } = true;

        /// <summary>
        /// The amount of seconds to delay before actually removing the data.
        /// </summary>
        public float RemoveDelay { get; set; } = 2f;

        /// <summary>
        /// The cacher instance which the data is loaded from.
        /// </summary>
        public ICacher<TKey, TValue> Cacher => cacher;

        /// <summary>
        /// The key currently in use.
        /// </summary>
        public TKey CurrentKey => lastKey;


        public CacherAgent(ICacher<TKey, TValue> cacher)
        {
            if (cacher == null) throw new ArgumentNullException(nameof(cacher));

            this.cacher = cacher;

            OnFinished += (value) => lastId = 0;
        }

        /// <summary>
        /// Requests for the cache data using specified key.
        /// </summary>
        public void Request(TKey key)
        {
			if(key == null) throw new ArgumentNullException(nameof(key));

            // Remove last data or request first.
            if(lastKey != null) Remove();

            lastKey = key;
            lastId = cacher.Request(key, this);
        }

        /// <summary>
        /// Removes the last cached data fetched from the cacher using the last specified key.
        /// </summary>
        public void Remove()
        {
			if(lastId > 0 || lastKey != null)
			{
				if(UseDelayedRemove)
					cacher.RemoveDelayed(lastKey, lastId, RemoveDelay);
				else
					cacher.Remove(lastKey, lastId);
			}

            // Reset progress
            ResetState();
            lastKey = null;
            lastId = 0;
        }
    }

    public class CacherAgent<T> : CacherAgent<string, T>
        where T : class
    {
        public CacherAgent(ICacher<string, T> cacher) : base(cacher)
        {
        }
    }
}