using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Allocation.Recyclers
{
    public class Recycler<T> : IRecycler<T> where T : class, IRecyclable<T> {

        /// <summary>
        /// List of all objects currently stored idle.
        /// </summary>
        private List<T> unusedObjects = new List<T>();

        /// <summary>
        /// Function which instantiates a new instance of T.
        /// </summary>
        private CreateHandler instantiator;


        /// <summary>
        /// Delegate for instantiating a new object T.
        /// </summary>
        public delegate T CreateHandler();


        public int TotalCount { get; private set; }

        public int UnusedCount => unusedObjects.Count;


        public Recycler(CreateHandler instantiator)
        {
            if(instantiator == null)
                throw new ArgumentNullException(nameof(instantiator));

            this.instantiator = instantiator;
        }

        public void Precook(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = CreateObject();
                Return(obj);
            }
        }

        public virtual T GetNext()
        {
            T obj = GetAvailable();
            obj.OnRecycleNew();
            return obj;
        }

        public virtual void Return(T obj)
        {
            obj.OnRecycleDestroy();
            unusedObjects.Add(obj);
        }

        /// <summary>
        /// Returns the next available item in the pool, or creates a new one.
        /// </summary>
        private T GetAvailable()
        {
            // Create one if not available.
            if (unusedObjects.Count == 0)
            {
                return CreateObject();
            }

            // Or use unused object if exists.
            int lastIndex = unusedObjects.Count - 1;
            var next = unusedObjects[lastIndex];
            unusedObjects.RemoveAt(lastIndex);
            return next;
        }

        /// <summary>
        /// Creates a new item using the instantiator delegate.
        /// </summary>
        private T CreateObject()
        {
            TotalCount++;
            
            var obj = instantiator.Invoke();
            obj.Recycler = this;
            return obj;
        }
    }
}