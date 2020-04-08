using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data.Bindables
{
    /// <summary>
    /// ProxyBindable implementation for numbers.
    /// </summary>
    public abstract class ProxyBindableNumber<T> : ProxyBindable<T>, IBindableNumber<T>
        where T : struct
    {
        private T min;
        private T max;


        public T MinValue
        {
            get => min;
            set
            {
                // Set min
                if (GreaterThan(value, max))
                    min = max;
                else
                    min = value;

                // Adjust value
                if (GreaterThan(min, this.Value))
                    Value = min;
            }
        }

        public T MaxValue
        {
            get => max;
            set
            {
                // Set max
                if (LessThan(value, min))
                    max = min;
                else
                    max = value;

                // Adjust value
                if (LessThan(max, this.Value))
                    Value = max;
            }
        }

        public override T Value
        {
            get => base.Value;
            set
            {
                if (LessThan(value, min))
                    base.Value = min;
                else if (GreaterThan(value, max))
                    base.Value = max;
                else
                    base.Value = value;
            }
        }


        public ProxyBindableNumber(Func<T> getter, Action<T> setter, T min, T max) : base(getter, setter)
        {
            // Set min/max bounds
            this.min = min;
            this.max = max;
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>
        /// Returns whether x is less than y.
        /// </summary>
        protected abstract bool LessThan(T x, T y);

        /// <summary>
        /// Returns whether x is greater than y.
        /// </summary>
        protected abstract bool GreaterThan(T x, T y);

        /// <summary>
        /// Returns whether x is equal to y.
        /// </summary>
        protected abstract bool EqualTo(T x, T y);
    }
}