using System;

namespace PBFramework.Data.Bindables
{
    public interface IBindable
    {
        /// <summary>
        /// Events called when the value of the bindable has changed to a new state.
        /// This event allows listening to state changes without the need to receive the previous state.
        /// </summary>
        event Action<object> OnNewRawValue;

        /// <summary>
        /// Events called when the inner value has changed.
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
        /// Binds the specified new value event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<object> callback);

        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<object, object> callback);
    }

    public interface IBindable<T> : IBindable
    {
        /// <summary>
        /// Events called when the value of the bindable has changed to a new state.
        /// This event allows listening to state changes without the need to receive the previous state.
        /// </summary>
        event Action<T> OnNewValue;

        /// <summary>
        /// Events called when the inner value has changed.
        /// </summary>
        event Action<T, T> OnValueChanged;


        /// <summary>
        /// The inner value stored.
        /// </summary>
        T Value { get; set; }


        /// <summary>
        /// Binds the specified new value event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<T> callback);

        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<T, T> callback);
    }
}
