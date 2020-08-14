using System;
using System.Threading.Tasks;

namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// Variant of ProxyFuture which handles the inner task in a separate thread.
    /// 
    /// </summary>
    public class AsyncFuture : ProxyFuture {
    
        public override TaskHandler Handler
        {
            get => base.Handler;
            set
            {
                base.Handler = (future) => RunHandlerAsync(value);
            }
        }

        public AsyncFuture(TaskHandler handler)
        {
            this.Handler = handler;
        }

        /// <summary>
        /// Starts running the specified handler asynchronously.
        /// </summary>
        private Task RunHandlerAsync(TaskHandler handler)
        {
            return Task.Run(() =>
            {
                if(handler == null)
                    StartRunning(null);
                else
                    InvokeHandler(handler);
            });
        }
    }
}