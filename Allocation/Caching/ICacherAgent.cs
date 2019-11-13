using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Interface of a cacher agent which encapsulates resource loading, destruction,
    /// and management of data and load ID away from consumer for cleaner code.
    /// Note that the implementation should assume service only for a single consumer.
    /// </summary>
    public interface ICacherAgent<TKey, TValue> : IReturnableProgress<TValue>
        where TKey : class
        where TValue: class
    {
        /// <summary>
        /// Whether removing data should be done through delayed removal.
        /// </summary>
        bool UseDelayedRemove { get; set; }

        /// <summary>
        /// The amount of seconds to delay before actually removing the data.
        /// </summary>
        float RemoveDelay { get; set; }

        /// <summary>
        /// The cacher instance which the data is loaded from.
        /// </summary>
        ICacher<TKey, TValue> Cacher { get; }

        /// <summary>
        /// The key currently in use.
        /// </summary>
        TKey CurrentKey { get; }


        /// <summary>
        /// Requests for the cache data using specified key.
        /// </summary>
        void Request(TKey key);

        /// <summary>
        /// Removes the last cached data fetched from the cacher using the last specified key.
        /// </summary>
        void Remove();
    }
}