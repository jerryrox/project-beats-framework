using System;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.Data.Bindables
{
    public class Bindable<T> : IBindable<T>
    {
        public event Action<T> OnValueChanged;

        public event Action<object> OnRawValueChanged;

        /// <summary>
        /// The actual value stored.
        /// </summary>
        protected T value;

        /// <summary>
        /// Whether the type parameter T is nullable.
        /// </summary>
        private bool isNullableT;


        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                Trigger();
            }
        }

        public object RawValue
        {
            get => Value;
            set
            {
                // If null is not allowed, throw
                if(value == null && !isNullableT)
                    throw new ArgumentException($"Bindable.RawValue - Type ({typeof(T).Name}) is a value type, but a null value was passed!");
                // Make sure the value is convertible to T.
                if(value is T val)
                    Value = val;
                else
                    throw new ArgumentException($"Bindable.RawValue - Expected type of ({typeof(T).Name}), but ({value.GetType().Name}) was given!");
            }
        }


        public Bindable() : this(default) {}

        public Bindable(T value)
        {
            this.value = value;

            isNullableT = !typeof(T).IsValueType;
        }

        public void Trigger()
        {
            OnValueChanged?.Invoke(value);
            OnRawValueChanged?.Invoke(value);
        }

        public virtual void Parse(string value)
        {
            // There shouldn't be any parsing function for generic Bindable type.
            Logger.LogWarning($"Bindable.Parse - Attempted to parse when implementation was not specified.");
        }

        public override string ToString()
        {
            if(isNullableT && value == null)
                return "null";
            return value.ToString();
        }
    }
}
