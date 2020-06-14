namespace PBFramework.Allocation.Recyclers
{
    /// <summary>
    /// Interface of a recyclable object without recycler defined.
    /// </summary>
    public interface IRecyclable
    {
        /// <summary>
        /// Event called from IRecycler when this object is about to be renewed for new use.
        /// </summary>
        void OnRecycleNew();

        /// <summary>
        /// Event called from IRecycler when this object is about to be destroyed for later use.
        /// </summary>
        void OnRecycleDestroy();
    }

    /// <summary>
    /// Indicates that the object can be recycled using IRecycler.
    /// </summary>
    public interface IRecyclable<T> : IRecyclable
        where T : class, IRecyclable<T>
    {
        /// <summary>
        /// The recycler instance which this object is being managed by.
        /// </summary>
        IRecycler<T> Recycler { get; set; }
    }
}