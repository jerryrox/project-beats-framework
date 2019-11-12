using System;
using PBFramework.Threading;

namespace PBFramework.Allocation
{
    /// <summary>
    /// Indicates that the object provides cached data allocation service using a customized key.
    /// </summary>
    public interface ICacher<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// Requests for a data with specified key.
        /// Returns a hook ID which uniquely identifies the IProgress passed as parameter.
        /// </summary>
        uint Request(TKey key, IReturnableProgress<TValue> progress);

        /// <summary>
        /// Attempts to remove the data associated with specified key immediately.
        /// ID may be passed as 0 if the data has already been fully loaded.
        /// </summary>
        void Remove(TKey key, uint id);

        /// <summary>
        /// Attempts to remove the data associated with specified key after a delay.
        /// This method should be preferred over Remove if the data needs to be loaded again within a short amount of time.
        /// </summary>
        void RemoveDelayed(TKey key, uint id, float delay = 2f);

        /// <summary>
        /// Returns whether 
        /// </summary>
        bool IsCached(TKey key);

        /// <summary>
        /// Converts the specified TKey value to a unique string key.
        /// </summary>
        string StringifyKey(TKey key);
    }

    /// <summary>
    /// Indicates that the object provides cached resource allocation service using a default string-typed key.
    /// </summary>
    public interface ICacher<T> : ICacher<string, T> where T : class {

    }
}