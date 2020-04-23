using System.Collections.Generic;

namespace PBFramework.Allocation.Recyclers
{
    /// <summary>
    /// Recycler implementation which includes management of active objects list.
    /// </summary>
    public class ManagedRecycler<T> : Recycler<T>
        where T : class, IRecyclable<T>
    {
        public List<T> ActiveObjects { get; private set; } = new List<T>();

        public int ActiveCount => ActiveObjects.Count;


        public ManagedRecycler(CreateHandler handler) : base(handler) {}

        public override T GetNext()
        {
            var t = base.GetNext();
            ActiveObjects.Add(t);
            return t;
        }

        public override void Return(T obj)
        {
            ActiveObjects.Remove(obj);
            base.Return(obj);
        }

        /// <summary>
        /// Returns the object at specified index.
        /// </summary>
        public void Return(int index)
        {
            var item = ActiveObjects[index];
            ActiveObjects.RemoveAt(index);
            base.Return(item);
        }

        /// <summary>
        /// Returns all items in the active objects list.
        /// </summary>
        public void ReturnAll()
        {
            foreach(var obj in ActiveObjects)
                base.Return(obj);
            ActiveObjects.Clear();
        }
    }
}