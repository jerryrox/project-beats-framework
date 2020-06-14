using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data
{
    /// <summary>
    /// Data structure which encalsulates the process of caching/invalidating a certain data type.
    /// </summary>
    public class Cached<T> {

        private T cachedValue;


        /// <summary>
        /// Delegate invoked on retrieving the cached value when the value is currently invalid.
        /// Ignored if null.
        /// </summary>
        public Func<T> OnRenewValue { get; set; }

        /// <summary>
        /// The value currently cached.
        /// </summary>
        public T Value
        {
            get
            {
                if(IsValid)
                    return cachedValue;
                if(OnRenewValue != null)
                    Value = OnRenewValue.Invoke();
                if(DefaultOnInvalidate)
                    return DefaultValue;
                return cachedValue;
            }
            set
            {
                cachedValue = value;
                IsValid = true;
            }
        }

        /// <summary>
        /// The default value to use for type T.
        /// </summary>
        public T DefaultValue { get; set; } = default;

        /// <summary>
        /// Whether the value currently cached is valid.
        /// </summary>
        public bool IsValid { get; private set; } = false;

        /// <summary>
        /// Whether the cached type's default value should be returned if invalid and RenewValue delegate is not specified.
        /// </summary>
        public bool DefaultOnInvalidate { get; set; } = true;


        public Cached() : this(default, false) {}

        public Cached(T initialValue) : this(initialValue, true) { }

        public Cached(T initialValue, bool isValid)
        {
            cachedValue = initialValue;
            IsValid = isValid;
        }

        /// <summary>
        /// Invalidates the value currently stored.
        /// </summary>
        public void Invalidate() => IsValid = false;
    }
}