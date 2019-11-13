using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading
{
    public class ReturnableProgress<T> : SimpleProgress, IReturnableProgress<T> {

        public event Action<T> OnFinished;
        event Action IEventProgress.OnFinished
        {
            add => OnFinished += delegate { value(); };
            remove => OnFinished -= delegate { value(); };
        }


        object IReturnableProgress.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        public T Value { get; set; }


        public virtual void InvokeFinished(T value)
        {
            Value = value;
            OnFinished?.Invoke(value);
        }

        public virtual void InvokeFinished(object value) => InvokeFinished((T)value);

        public virtual void InvokeFinished() => InvokeFinished(default(T));
    }
}