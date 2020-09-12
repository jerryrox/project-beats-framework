using PBFramework.Threading;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Derivant of TaskListener to yield a Key to be used with Cacher.
    /// </summary>
    public class CacheListener<T> : TaskListener<T>
    {
        /// <summary>
        /// The key associated with this listener when a request for a cached resource was made.
        /// </summary>
        public object Key { get; private set; }


        /// <summary>
        /// Initializes a new CacheListener with a key.
        /// </summary>
        public CacheListener(object key)
        {
            this.Key = key;
        }
    }
}