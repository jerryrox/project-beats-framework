using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    public class CacherAgent<TKey, TValue> : ReturnableProgress<TValue>, ICacherAgent<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private ICacher<TKey, TValue> cacher;

        private TKey lastKey;
        private uint lastId;


        public bool UseDelayedRemove { get; set; } = true;

        public float RemoveDelay { get; set; } = 2f;

        public ICacher<TKey, TValue> Cacher => cacher;

        public TKey CurrentKey => lastKey;


        public CacherAgent(ICacher<TKey, TValue> cacher)
        {
            if(cacher == null) throw new ArgumentNullException(nameof(cacher));

            this.cacher = cacher;
        }

        public void Request(TKey key)
        {
			if(key == null) throw new ArgumentNullException(nameof(key));

            // Remove last data or request first.
            if(lastKey != null) Remove();

            lastKey = key;
            lastId = cacher.Request(key, this);
        }

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
            Report(0);

            Value = null;
            lastKey = null;
            lastId = 0;
        }

        public override void InvokeFinished(TValue value)
        {
            lastId = 0;
            base.InvokeFinished(value);
        }

        public override void InvokeFinished() => throw new NotSupportedException();
    }

    public class CacherAgent<T> : CacherAgent<string, T>
        where T : class
    {
        public CacherAgent(ICacher<string, T> cacher) : base(cacher)
        {
        }
    }
}