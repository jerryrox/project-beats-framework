using System.Collections.Generic;

namespace PBFramework.Allocation.Recyclers
{
    /// <summary>
    /// Indicates that the object provides recycling logics.
    /// </summary>
    public interface IRecycler<T> where T : class, IRecyclable<T> {

        /// <summary>
        /// Returns the total number of objects managed by this recycler.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Returns the number of objects currently disabled and stored in this recycler.
        /// </summary>
        int UnusedCount { get; }

        /// <summary>
        /// Returns the list of all objects currently unused.
        /// </summary>
        List<T> UnusedObjects { get; }


        /// <summary>
        /// Pre-instantiates specified number of objects and disable them immediately.
        /// </summary>
        void Precook(int count);

        /// <summary>
        /// Returns the next available object in the recycler.
        /// </summary>
        T GetNext();

        /// <summary>
        /// Returns the specified object 
        /// </summary>
        void Return(T obj);
    }
}