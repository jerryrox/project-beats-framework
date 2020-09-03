using System;

namespace PBFramework.Allocation.Caching
{
    public class CacherAgent<TKey, TValue> : ICacherAgent<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        public bool UseDelayedRemove { get; set; } = true;

        public float RemoveDelay { get; set; } = 2f;

        public ICacher<TKey, TValue> Cacher { get; private set; }

        public CacheListener<TKey, TValue> Listener { get; private set; }


        public CacherAgent(ICacher<TKey, TValue> cacher)
        {
            if(cacher == null) throw new ArgumentNullException(nameof(cacher));

            this.Cacher = cacher;
        }

        public CacheListener<TKey, TValue> Request(TKey key)
        {
			if(key == null) throw new ArgumentNullException(nameof(key));

            // Remove last data or request first.
            if(Listener != null)
                Remove();

            return Listener = Cacher.Request(key);
        }

        public void Remove()
        {
            if(Listener != null)
			{
				if(UseDelayedRemove)
					Cacher.RemoveDelayed(Listener, RemoveDelay);
				else
					Cacher.Remove(Listener);
			}
            Listener = null;
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