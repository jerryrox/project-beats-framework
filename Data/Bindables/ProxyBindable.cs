using System;
using System.Collections.Generic;

namespace PBFramework.Data.Bindables
{
    /// <summary>
    /// Extension of Bindable which assumes the data source to be external from Bindable, and
    /// must be accessed and modified through delegates.
    /// </summary>
    public class ProxyBindable<T> : Bindable<T> {

        protected Func<T> proxyGetter;
        protected Action<T> proxySetter;


        public override T Value
        {
            get => proxyGetter.Invoke();
            set
            {
                T oldValue = proxyGetter.Invoke();
                // If triggers only when different, but the values are the same, just return.
                if (TriggerWhenDifferent && EqualityComparer<T>.Default.Equals(oldValue, value))
                    return;
                SetValueInternal(value);
                // Even if the requested value for setting may seem valid,
                // the actual setter function may treat it as invalid and clamp it to a reasonable value.
                // For this purpose, we should trigger update using the value through proxyGetter and not value.
                TriggerInternal(proxyGetter.Invoke(), oldValue);
            }
        }


        public ProxyBindable(Func<T> getter, Action<T> setter)
        {
            if(getter == null) throw new ArgumentNullException(nameof(getter));
            if(setter == null) throw new ArgumentNullException(nameof(setter));

            proxyGetter = getter;
            proxySetter = setter;
        }

        protected override void SetValueInternal(T value) => proxySetter.Invoke(value);
    }
}