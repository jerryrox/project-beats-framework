using PBFramework.Threading.Futures;

namespace PBFramework.Allocation.Caching
{
    public class CacheListener<TKey, TValue> : ProxyFuture<TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// The key associated with this listener when a request for a cached resource was made.
        /// </summary>
        public TKey Key { get; private set; }


        /// <summary>
        /// Initializes a new CacheListener with an on-going request.
        /// </summary>
        public CacheListener(TKey key, IFuture<TValue> request) : base(request)
        {
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new CacheListener with an output value and a completed state.
        /// </summary>
        public CacheListener(TKey key, TValue value) :
            base(new Future<TValue>((f) => f.SetComplete(value)))
        {
            this.Key = key;
            (base.future as Future<TValue>).Start();
        }
    }
}