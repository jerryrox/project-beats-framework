using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// Non-generic version of ProxyFuture with object type parameter.
    /// </summary>
    public class ProxyFuture : ProxyFuture<object>, IControlledFuture {

        public ProxyFuture() : base(null)
        {
        }

        public ProxyFuture(TaskHandler handler) : base(handler)
        {
        }
    }

    /// <summary>
    /// Implementation of future which encapsualtes the given task into a future.
    /// Automatically handles catching exception while running the given task.
    /// </summary>
    public class ProxyFuture<T> : Future<T>, IControlledFuture<T>
    {
        /// <summary>
        /// Delegate for handling a task under this future's context.
        /// </summary>
        public delegate void TaskHandler(ProxyFuture<T> future);

        private TaskHandler handler;


        /// <summary>
        /// The inner task of this Future instance.
        /// </summary>
        public virtual TaskHandler Handler
        {
            get => handler;
            set
            {
                AssertNotRun("Can not assign task handler to a running future.");
                handler = value;
            }
        }


        /// <summary>
        /// Initializes the Future instance with an empty task.
        /// </summary>
        public ProxyFuture() : this(null) {}

        /// <summary>
        /// Initializes the Future instance with the specified handler.
        /// </summary>
        public ProxyFuture(TaskHandler handler)
        {
            this.handler = handler;
        }

        public void Start()
        {
            if (handler == null)
                StartRunning(null);
            else
                StartRunning(() => InvokeHandler(handler));
        }

        /// <summary>
        /// Sets the Future to complete state.
        /// </summary>
        public void SetComplete() => base.OnComplete(null);

        /// <summary>
        /// Sets the Future to completed state with a value.
        /// </summary>
        public void SetComplete(T value) => base.OnComplete(value, null);

        /// <summary>
        /// Sets the Future to complete state with an error.
        /// </summary>
        public void SetFail(Exception e) => base.OnComplete(e);

        /// <summary>
        /// Sets the progress state.
        /// </summary>
        public void SetProgress(float progress) => base.ReportProgress(progress);

        /// <summary>
        /// Invokes the specified handler within a try/catch context.
        /// </summary>
        protected void InvokeHandler(TaskHandler handler)
        {
            try
            {
                handler.Invoke(this);
            }
            catch (Exception e)
            {
                SetFail(e);
            }
        }
    }
}