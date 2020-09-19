using System;
using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Indicates that the object provides cached data allocation service using a customized key.
    /// </summary>
    public interface ICacher<TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// Requests for a data with specified key.
        /// Returns a listener instance the consumer can listen to.
        /// </summary>
        CacheListener<TValue> Request(TKey key);

        /// <summary>
        /// Attempts to remove the data associated with specified listener immediately.
        /// </summary>
        void Remove(CacheListener<TValue> listener);

        /// <summary>
        /// Attempts to remove the data associated with specified key after a delay.
        /// This method should be preferred over Remove if the data needs to be loaded again within a short amount of time.
        /// </summary>
        void RemoveDelayed(CacheListener<TValue> listener, float delay = 2f);

        /// <summary>
        /// Returns whether a resource with the specified key has been cached.
        /// </summary>
        bool IsCached(TKey key);
    }
}