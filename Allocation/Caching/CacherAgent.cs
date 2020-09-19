using System;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Class which encapsulates resource loading, destruction,
    /// and management of data and load ID away from consumer for cleaner code.
    /// </summary>
    public class CacherAgent<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// Event called when the last request object has been retrieved.
        /// </summary>
        public event Action<TValue> OnFinished;

        /// <summary>
        /// Event called when the last request has a new progress to report.
        /// </summary>
        public event Action<float> OnProgress;

        /// <summary>
        /// Whether removing data should be done through delayed removal.
        /// </summary>
        public bool UseDelayedRemove { get; set; } = true;

        /// <summary>
        /// The amount of seconds to delay before actually removing the data.
        /// </summary>
        public float RemoveDelay { get; set; } = 2f;

        /// <summary>
        /// The last key used to make a cacher request.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// The cacher instance which the data is loaded from.
        /// </summary>
        public ICacher<TKey, TValue> Cacher { get; private set; }

        /// <summary>
        /// The current object listening to cacher request.
        /// </summary>
        public CacheListener<TValue> Listener { get; private set; }


        public CacherAgent(ICacher<TKey, TValue> cacher)
        {
            if (cacher == null)
                throw new ArgumentNullException(nameof(cacher));

            this.Cacher = cacher;
        }

        /// <summary>
        /// Requests for the cache data using specified key.
        /// </summary>
        public CacheListener<TValue> Request(TKey key)
        {
			if(key == null)
                throw new ArgumentNullException(nameof(key));

            // Remove last data or request first.
            if(Listener != null)
                Remove();

            Key = key;
            Listener = Cacher.Request(key);
            if (Listener.IsFinished)
            {
                InvokeFinished(Listener.Value);
                InvokeProgress(1f);
            }
            else
            {
                Listener.OnFinished += InvokeFinished;
                Listener.OnProgress -= InvokeProgress;
            }
            return Listener;
        }

        /// <summary>
        /// Removes the last cached data fetched from the cacher using the last specified key.
        /// </summary>
        public void Remove()
        {
			if(Listener != null)
			{
                Listener.OnFinished -= InvokeFinished;
                Listener.OnProgress -= InvokeProgress;

                if(UseDelayedRemove)
					Cacher.RemoveDelayed(Listener, RemoveDelay);
				else
					Cacher.Remove(Listener);
			}
            Key = null;
            Listener = null;
        }

        /// <summary>
        /// Invokes the OnFinished event.
        /// </summary>
        private void InvokeFinished(TValue value)
        {
            OnFinished?.Invoke(value);
        }

        /// <summary>
        /// Invokes the OnProgress event.
        /// </summary>
        private void InvokeProgress(float value)
        {
            OnProgress?.Invoke(value);
        }
    }
}