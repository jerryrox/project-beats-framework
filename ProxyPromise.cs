using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework
{
    /// <summary>
    /// An anonymous promise which processes and determines completion via external functions.
    /// </summary>
    public class ProxyPromise : IExplicitPromise
    {
        public event Action OnFinished;

        public event Action<float> OnProgress;

        protected Action startAction;
        protected Action revokeAction;


        public object RawResult { get; protected set; } = null;

        public bool IsFinished { get; protected set; } = false;

        public float Progress { get; protected set; }


        /// <summary>
        /// Initializes a new promise with an empty process so that it resolves immediately on start.
        /// </summary>
        public ProxyPromise()
        {
            startAction = () => Resolve(null);
        }

        public ProxyPromise(Action<Action<object>> startActionWithResolve)
        {
            if(startActionWithResolve != null)
                this.startAction = () => startActionWithResolve.Invoke(new Action<object>(Resolve));
        }

        public ProxyPromise(Action startAction = null, Action revokeAction = null)
        {
            this.startAction = startAction;
            this.revokeAction = revokeAction;
        }

        /// <summary>
        /// Sets the progress of the external task and fires the progress event.
        /// </summary>
        public void SetProgress(float progress)
        {
            this.Progress = progress;
            OnProgress?.Invoke(progress);
        }

        /// <summary>
        /// Stores the specified value as result and fires the finished event.
        /// </summary>
        public virtual void Resolve(object value)
        {
            RawResult = value;
            IsFinished = true;
            OnFinished?.Invoke();
        }

        public void Start() => startAction?.Invoke();

        public void Revoke() => revokeAction?.Invoke();
    }

    /// <summary>
    /// Generic support for ProxyPromise.
    /// </summary>
    public class ProxyPromise<T> : ProxyPromise, IExplicitPromise<T>
    {
        public event Action<T> OnFinishedResult
        {
            add => OnFinished += () => value(Result);
            remove => OnFinished -= () => value(Result);
        }


        public T Result { get; protected set; }


        public ProxyPromise(Action<Action<T>> startActionWithResolve)
        {
            if (startActionWithResolve != null)
                this.startAction = () => startActionWithResolve.Invoke(new Action<T>(Resolve));
        }

        public ProxyPromise(Action startAction = null, Action revokeAction = null) :
            base(startAction, revokeAction)
        {
        }

        /// <summary>
        /// Stores the specified value as result and fires the finished event.
        /// </summary>
        public void Resolve(T value)
        {
            Result = value;
            base.Resolve(value);
        }

        public override void Resolve(object value)
        {
            if(!(value is T tValue)) throw new ArgumentException($"value must be a type of {nameof(T)}");
            Resolve(tValue);
        }
    }
}