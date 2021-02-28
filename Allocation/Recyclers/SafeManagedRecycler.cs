namespace PBFramework.Allocation.Recyclers
{
    /// <summary>
    /// ManagedRecycler subclass which adds thread safety to it.
    /// </summary>
    public class SafeManagedRecycler<T> : ManagedRecycler<T>
        where T : class, IRecyclable<T>
    {
        private object locker = new object();


        public SafeManagedRecycler(CreateHandler handler) : base(handler) { }

        public override T GetNext()
        {
            lock (locker)
            {
                return base.GetNext();
            }
        }

        public override void Return(T obj)
        {
            lock (locker)
            {
                base.Return(obj);
            }
        }

        public override void Return(int index)
        {
            lock (locker)
            {
                base.Return(index);
            }
        }

        public override void ReturnAll()
        {
            lock (locker)
            {
                base.ReturnAll();
            }
        }
    }
}