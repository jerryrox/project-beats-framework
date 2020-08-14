using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading.Futures
{
    public abstract class Future : IFuture
    {
        private BindableFloat progress = new BindableFloat(0f)
        {
            TriggerWhenDifferent = true
        };
        private BindableBool isDisposed = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };
        private BindableBool isCompleted = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };
        private Bindable<Exception> error = new Bindable<Exception>(null)
        {
            TriggerWhenDifferent = true
        };

        private bool isThreadSafe = true;

        private Action continuation;


        public IReadOnlyBindable<float> Progress => progress;

        public IReadOnlyBindable<bool> IsDisposed => isDisposed;

        public IReadOnlyBindable<bool> IsCompleted => isCompleted;

        public IReadOnlyBindable<Exception> Error => error;

        // TODO: Implement thread safe boolean type.
        public bool IsThreadSafe
        {
            get
            {
                return isThreadSafe;
            }
            set
            {
                isThreadSafe = value;
            }
        }

        public bool DidRun { get; private set; }


        public virtual void Dispose()
        {
            AssertNotDisposed();

            RunWithThreadSafety(() =>
            {
                isDisposed.Value = true;
            });
        }

        void INotifyCompletion.OnCompleted(Action continuation) => OnReceiveContinuation(continuation);

        /// <summary>
        /// Starts running the inner task of the future.
        /// </summary>
        protected void StartRunning(Action runAction)
        {
            AssertNotDisposed();
            AssertNotRun();
            AssertNotCompleted();

            RunWithThreadSafety(() =>
            {
                DidRun = true;

                if (runAction == null)
                    OnRunEmptyAction();
                else
                    runAction.Invoke();
            });
        }

        /// <summary>
        /// Changes the current progress state.
        /// </summary>
        protected void ReportProgress(float progress)
        {
            RunWithThreadSafety(() =>
            {
                this.progress.Value = progress;
            });
        }

        /// <summary>
        /// Event called from StartRunning when the action to run is empty.
        /// </summary>
        protected virtual void OnRunEmptyAction()
        {
            ReportProgress(1f);
            OnComplete(null);
        }

        /// <summary>
        /// Receives async/await continuation callback.
        /// </summary>
        protected virtual void OnReceiveContinuation(Action continuation)
        {
            AssertNotDisposed();

            RunWithThreadSafety(() =>
            {
                this.continuation = continuation;
                if (isCompleted.Value)
                    continuation?.Invoke();
            });
        }

        /// <summary>
        /// Event that should be called when the inner task has finished its job.
        /// </summary>
        protected void OnComplete(Exception error)
        {
            if(isDisposed.Value)
                return;
            AssertNotCompleted();

            RunWithThreadSafety(() =>
            {
                ReportProgress(1f);
                this.isCompleted.Value = true;
                this.error.Value = error;

                continuation?.Invoke();
            });
        }

        /// <summary>
        /// Makes sure this object has not been disposed.
        /// </summary>
        protected void AssertNotDisposed()
        {
            if (isDisposed.Value)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Makes sure this object has not yet run its inner task.
        /// </summary>
        protected void AssertNotRun(string message = null)
        {
            if (DidRun)
                throw new Exception(message ?? "This Future instance has already been run once!");
        }

        /// <summary>
        /// Makes sure this object has not yet completed the inner task.
        /// </summary>
        protected void AssertNotCompleted()
        {
            if(isCompleted.Value)
                throw new Exception("This Future's task has already been completed!");
        }

        /// <summary>
        /// Runs the specified action in thread-safe manner if IsThreadSafe flag is true.
        /// </summary>
        protected void RunWithThreadSafety(Action action)
        {
            if (IsThreadSafe)
            {
                UnityThread.DispatchUnattended(() =>
                {
                    action.Invoke();
                    return null;
                });
            }
            else
            {
                action.Invoke();
            }
        }
    }

    public abstract class Future<T> : Future, IFuture<T>
    {
        protected Bindable<T> output = new Bindable<T>()
        {
            TriggerWhenDifferent = true
        };


        public IReadOnlyBindable<T> Output => output;


        /// <summary>
        /// Event that should be called when the inner task has finished its job.
        /// </summary>
        protected void OnComplete(T output, Exception error)
        {
            if(IsDisposed.Value)
                return;
            AssertNotCompleted();

            RunWithThreadSafety(() =>
            {
                this.output.Value = output;
                base.OnComplete(error);
            });
        }
    }
}