using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// Future which relies on another future.
    /// </summary>
    public class ProxyFuture : Future
    {
        private IFuture future;


        /// <summary>
        /// Creates a ProxyFuture with another future assuming it is currently on-going.
        /// </summary>
        public ProxyFuture(IFuture future)
        {
            if(future == null)
                throw new ArgumentNullException(nameof(future));
            if(!future.DidRun)
                throw new Exception("ProxyFuture created with an IFuture must be in a running state.");

            InitWithFuture(future);
        }

        /// <summary>
        /// Creates a ProxyFuture with another future assuming it may (not) be currently on-going.
        /// </summary>
        public ProxyFuture(IControlledFuture future)
        {
            if(future == null)
                throw new ArgumentNullException(nameof(future));

            InitWithFuture(future);
        }

        public override void Dispose()
        {
            base.Dispose();

            RunWithThreadSafety(() =>
            {
                UnbindEvents();
                future = null;
            });
        }

        public override void SetComplete() => AssertNotModifiable();

        public override void SetFail(Exception e) => AssertNotModifiable();

        public override void SetProgress(float progress) => AssertNotModifiable();

        /// <summary>
        /// Binds this object's states to specified future's states.
        /// </summary>
        protected virtual void BindEvents()
        {
            future.IsDisposed.BindAndTrigger(OnOtherDisposed);
            // If the future was already disposed, just return.
            if(this.IsDisposed.Value)
                return;

            future.Progress.BindAndTrigger(OnOtherProgress);
            future.IsCompleted.BindAndTrigger(OnOtherCompleted);
        }

        /// <summary>
        /// Binds this object's states to specified future's states.
        /// </summary>
        protected virtual void UnbindEvents()
        {
            future.Progress.OnNewValue -= OnOtherProgress;
            future.IsDisposed.OnNewValue -= OnOtherDisposed;
            future.IsCompleted.OnNewValue -= OnOtherCompleted;
        }

        /// <summary>
        /// Throws an invalid operation exception to indicate that direct modification of ProxyFuture is not valid.
        /// </summary>
        protected void AssertNotModifiable()
        {
            throw new InvalidOperationException("ProxyFutures can only be modified through another future instance.");
        }

        /// <summary>
        /// Initializes the ProxyFuture with another future.
        /// </summary>
        private void InitWithFuture(IFuture future)
        {
            this.future = future;
            BindEvents();

            if (!this.IsDisposed.Value && !this.IsCompleted.Value)
            {
                SetHandler((f) => { });
                if (future.DidRun)
                    Start();
            }
        }

        /// <summary>
        /// Event called from other future when its progress is reported.
        /// </summary>
        private void OnOtherProgress(float progress) => base.SetProgress(progress);

        /// <summary>
        /// Event called from other future when its disposed state is reported.
        /// </summary>
        private void OnOtherDisposed(bool disposed)
        {
            if (disposed)
                Dispose();
        }

        /// <summary>
        /// Event called from other future when its completion state is reported.
        /// </summary>
        private void OnOtherCompleted(bool completed)
        {
            if (completed)
            {
                if(future.Error.Value != null)
                    base.SetFail(future.Error.Value);
                else
                    base.SetComplete();
            }
        }
    }
}