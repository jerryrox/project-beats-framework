using System;
using System.Threading.Tasks;

namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// Variant of Future which handles the inner task in a separate thread.
    /// </summary>
    public class AsyncFuture : Future
    {
        public AsyncFuture(TaskHandler handler) : base(handler)
        {            
        }

        protected override void SetHandler(TaskHandler value)
        {
            if(value == null)
                base.SetHandler(null);
            else
                base.SetHandler((f) => RunHandlerAsync(value));
        }

        /// <summary>
        /// Starts running the specified handler asynchronously.
        /// </summary>
        protected void RunHandlerAsync(TaskHandler handler)
        {
            Task.Run(() => InvokeHandler(handler));
        }
    }

    /// <summary>
    /// Generic version of AsyncFuture.
    /// Does not inherit from the non-generic AsyncFuture class.
    /// </summary>
    public class AsyncFuture<T> : Future<T>
    {

        public AsyncFuture(TaskHandlerT handler) : base(handler)
        {
        }

        protected override void SetHandler(TaskHandlerT value)
        {
            if (value == null)
                base.SetHandler(null);
            else
                base.SetHandler(BuildAsyncTaskHandler(value));
        }

        /// <summary>
        /// Builds an asynchronous version of generic task handler from specified handler.
        /// </summary>
        protected TaskHandlerT BuildAsyncTaskHandler(TaskHandlerT handler)
        {
            return (f) => RunHandlerAsync(handler);
        }

        /// <summary>
        /// Starts running the specified handler asynchronously.
        /// </summary>
        protected void RunHandlerAsync(TaskHandlerT handler)
        {
            Task.Run(() => InvokeHandler(BuildTaskHandler(handler)));
        }
    }
}