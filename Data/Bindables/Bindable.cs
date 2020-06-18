using System;
using System.Collections.Generic;
using PBFramework.Debugging;
using Newtonsoft.Json;

namespace PBFramework.Data.Bindables
{
    public class Bindable<T> : IReadOnlyBindable<T>, IReadOnlyBindable, IBindable<T>
    {
        public event Action<object> OnNewRawValue;
        public event Action<object, object> OnRawValueChanged;

        public event Action<T> OnNewValue;
        public event Action<T, T> OnValueChanged;

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
                T oldValue = this.value;
                // If triggers only when different, but the values are the same, just return.
                if (TriggerWhenDifferent && EqualityComparer<T>.Default.Equals(oldValue, value))
                    return;
                SetValueInternal(value);
                TriggerInternal(value, oldValue);
            }
        }

        T IReadOnlyBindable<T>.Value => Value;

        public bool TriggerWhenDifferent { get; set; } = false;

        [JsonIgnore]
        public object RawValue
        {
            get => Value;
            set
            {
                // If null is not allowed, throw
                if(value == null && !isNullableT)
                    throw new ArgumentException($"Bindable.RawValue - Type ({typeof(T).Name}) is a value type, but a null value was passed!");
                SetRawValueInternal(value, true);
            }
        }

        object IReadOnlyBindable.RawValue => Value;


        public Bindable() : this(default) {}

        public Bindable(T value)
        {
            this.value = value;

            isNullableT = !typeof(T).IsValueType;
        }

        public void Trigger() => TriggerInternal(Value, Value);

        public void TriggerWithPrevious(T previousValue) => TriggerInternal(Value, previousValue);

        public void SetWithoutTrigger(T value) => SetValueInternal(value);

        public void BindAndTrigger(Action<T> callback)
        {
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            OnNewValue += callback;
            callback.Invoke(Value);
        }

        public void BindAndTrigger(Action<T, T> callback)
        {
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            OnValueChanged += callback;
            callback.Invoke(Value, Value);
        }

        public virtual void Parse(string value)
        {
            // There shouldn't be any parsing function for generic Bindable type.
            Logger.LogWarning($"Bindable.Parse - Attempted to parse when implementation was not specified.");
        }

        public override string ToString()
        {
            if(isNullableT && Value == null)
                return "null";
            return Value.ToString();
        }

        /// <summary>
        /// Internally processes the Trigger routine.
        /// </summary>
        protected void TriggerInternal(T newValue, T oldValue)
        {
            OnNewValue?.Invoke(newValue);
            OnValueChanged?.Invoke(newValue, oldValue);
            OnNewRawValue?.Invoke(newValue);
            OnRawValueChanged?.Invoke(newValue, oldValue);
        }

        /// <summary>
        /// Sets the specified value to whichever state this bindable is referring to.
        /// </summary>
        protected virtual void SetValueInternal(T value) => this.value = value;

        /// <summary>
        /// Sets the specified value to whichever state this bindable is referring to.
        /// </summary>
        protected void SetRawValueInternal(object value, bool trigger)
        {
            // Make sure the value is convertible to T.
            if (value is T val)
            {
                if(trigger)
                    Value = val;
                else
                    SetValueInternal(val);
            }
            else
                throw new ArgumentException($"Bindable.SetValueInternal - Expected type of ({typeof(T).Name}), but ({value.GetType().Name}) was given!");
        }

        void IBindable.SetWithoutTrigger(object value) => SetRawValueInternal(value, false);

        void IBindable.BindAndTrigger(Action<object> callback)
        {
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            OnNewRawValue += callback;
            callback.Invoke(RawValue);
        }

        void IBindable.BindAndTrigger(Action<object, object> callback)
        {
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            OnRawValueChanged += callback;
            callback.Invoke(RawValue, RawValue);
        }
    }
}
