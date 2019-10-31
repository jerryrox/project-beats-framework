using System;

namespace PBFramework.Data.Bindables
{
    /// <summary>
    /// Bindable implementation for numbers.
    /// </summary>
    public abstract class BindableNumber<T> : Bindable<T> where T : struct
    {
        private T min;
        private T max;


        public T MinValue
        {
            get => min;
            set
            {
                // Set min
                if(GreaterThan(value, max))
                    min = max;
                else
                    min = value;

                // Adjust value
                if(GreaterThan(min, this.value))
                    Value = min;
            }
        }

        public T MaxValue
        {
            get => max;
            set
            {
                // Set max
                if(LessThan(value, min))
                    max = min;
                else
                    max = value;

                // Adjust value
                if(LessThan(max, this.value))
                    Value = max;
            }
        }

        public override T Value
        {
            get => base.Value;
            set
            {
                if(LessThan(value, min))
                    base.Value = min;
                else if(GreaterThan(value, max))
                    base.Value = max;
                else
                    base.Value = value;
            }
        }


        public BindableNumber(T value, T min, T max) : base(value)
        {
            // Set min and max bounds.
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