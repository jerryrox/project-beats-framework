using System;

namespace PBFramework.Data.Bindables
{
    public interface IBindable
    {
        /// <summary>
        /// Events called when the inner value has changed.
        /// Passes the new value and the previous value as parameter.
        /// </summary>
        event Action<object, object> OnRawValueChanged;


        /// <summary>
        /// The inner value stored in Object type.
        /// </summary>
        object RawValue { get; set; }

        /// <summary>
        /// Whether the triggering of value change should only occur when the new value is different.
        /// Default: false.
        /// </summary>
        bool TriggerWhenDifferent { get; set; }


        /// <summary>
        /// Triggers value changed event.
        /// </summary>
        void Trigger();

        /// <summary>
        /// Parses the specified string value into the type that matches the bound type.
        /// </summary>
        void Parse(string value);

        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<object, object> callback);
    }

    public interface IBindable<T> : IBindable
    {
        /// <summary>
        /// Events called when the inner value has changed.
        /// </summary>
        event Action<T, T> OnValueChanged;


        /// <summary>
        /// The inner value stored.
        /// </summary>
        T Value { get; set; }


        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<T, T> callback);
    }
}
