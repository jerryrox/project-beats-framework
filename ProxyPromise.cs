using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework
{
    /// <summary>
    /// An anonymous promise which processes and determines completion via external functions.
    /// </summary>
    public class ProxyPromise : IPromise
    {
        public event Action OnFinished;

        public event Action<float> OnProgress;

        protected Action startAction;
        protected Action revokeAction;


        public object Result { get; protected set; } = null;

        public bool IsFinished { get; protected set; } = false;

        public float Progress { get; protected set; }


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
            OnProgress?.Invoke(Progress = progress);
        }

        /// <summary>
        /// Stores the specified value as result and fires the finished event.
        /// </summary>
        public virtual void Resolve(object value)
        {
            Result = value;
            IsFinished = true;
            OnFinished?.Invoke();
        }

        public void Start() => startAction?.Invoke();

        public void Revoke() => revokeAction?.Invoke();
    }

    /// <summary>
    /// Generic support for ProxyPromise.
    /// </summary>
    public class ProxyPromise<T> : ProxyPromise, IPromise<T>
    {
        public event Action<T> OnFinishedResult
        {
            add => OnFinished += () => value(Result);
            remove => OnFinished -= () => value(Result);
        }


        public new T Result { get; protected set; }

    
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