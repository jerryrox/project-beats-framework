using PBFramework.Threading.Futures;

namespace PBFramework.Allocation.Caching
{
    public class CacheListener<T> : ProxyFuture<T>
        where T : class
    {
        /// <summary>
        /// The key associated with this listener when a request for a cached resource was made.
        /// </summary>
        public object Key { get; private set; }


        /// <summary>
        /// Initializes a new CacheListener with an on-going request.
        /// </summary>
        public CacheListener(object key, IFuture<T> request) : base(request)
        {
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new CacheListener with an output value and a completed state.
        /// </summary>
        public CacheListener(object key, T value) :
            base(new Future<T>((f) => f.SetComplete(value)))
        {
            this.Key = key;
            (base.future as Future<T>).Start();
        }
    }
}