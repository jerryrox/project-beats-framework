using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading.Futures
{
    public class Future : IFuture, IControlledFuture
    {
        /// <summary>
        /// Delegate for handling a task under this future's context.
        /// </summary>
        public delegate void TaskHandler(Future future);

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
        private TaskHandler handler;


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


        public Future(TaskHandler handler = null)
        {
            SetHandler(handler);
        }

        public void Start()
        {
            if (handler == null)
                StartRunning(null);
            else
                StartRunning(() => InvokeHandler(handler));
        }

        public virtual void Dispose()
        {
            if(isDisposed.Value)
                return;

            RunWithThreadSafety(() =>
            {
                isDisposed.Value = true;
            });
        }

        /// <summary>
        /// Sets the Future to complete state.
        /// </summary>
        public virtual void SetComplete() => OnComplete(null);

        /// <summary>
        /// Sets the Future to complete state with an error.
        /// </summary>
        public virtual void SetFail(Exception e) => OnComplete(e);

        /// <summary>
        /// Sets the progress state.
        /// </summary>
        public virtual void SetProgress(float progress) => ReportProgress(progress);

        /// <summary>
        /// Returns an awaiter to support async/await syntax.
        /// </summary>
        public FutureAwaiter GetAwaiter() => new FutureAwaiter(this, OnReceiveContinuation);

        /// <summary>
        /// Sets the inner task of the future.
        /// </summary>
        protected virtual void SetHandler(TaskHandler value)
        {
            AssertNotRun("Can not assign task handler to a running future.");
            handler = value;
        }

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
                this.error.Value = error;
                this.isCompleted.Value = true;

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

        /// <summary>
        /// Receives async/await continuation callback.
        /// </summary>
        private void OnReceiveContinuation(Action continuation)
        {
            AssertNotDisposed();

            RunWithThreadSafety(() =>
            {
                this.continuation = continuation;
                if (isCompleted.Value && continuation != null)
                    continuation.Invoke();
            });
        }
    }

    public class Future<T> : Future, IControlledFuture<T>
    {
        public delegate void TaskHandlerT(Future<T> future);

        protected Bindable<T> output = new Bindable<T>()
        {
            TriggerWhenDifferent = true
        };


        public IReadOnlyBindable<T> Output => output;


        /// <summary>
        /// Initializes the Future instance with the specified handler.
        /// </summary>
        public Future(TaskHandlerT handler = null)
        {
            SetHandler(handler);
        }

        /// <summary>
        /// Sets the inner task of the future.
        /// </summary>
        protected virtual void SetHandler(TaskHandlerT value)
        {
            if(value == null)
                base.SetHandler(null);
            else
                base.SetHandler(BuildTaskHandler(value));
        }

        /// <summary>
        /// Sets the Future to completed state with a value.
        /// </summary>
        public void SetComplete(T value) => OnComplete(value, null);

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

        /// <summary>
        /// Builds a non-generic version of TaskHandler from specified handler.
        /// </summary>
        protected TaskHandler BuildTaskHandler(TaskHandlerT handler)
        {
            return (f) => handler.Invoke(this);
        }
    }
}