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
        /// Sets the value of the bindable without triggering change event.
        /// </summary>
        void SetWithoutTrigger(object value);

        /// <summary>
        /// Makes the value of this bindable bound to the specified raw bindable.
        /// </summary>
        void BindToRaw(IBindable other);

        /// <summary>
        /// Makes the value of this bindable no longer bound to the specified raw bindable.
        /// </summary>
        void UnbindFromRaw(IBindable other);

        /// <summary>
        /// Binds the specified new value event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<object> callback);

        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<object, object> callback);

        /// <summary>
        /// Binds the specified new raw value event listener.
        /// </summary>
        void Bind(Action<object> callback);

        /// <summary>
        /// Binds the specified raw value changed event listener.
        /// </summary>
        void Bind(Action<object, object> callback);

        /// <summary>
        /// Unbinds the specified new raw value callback from this object.
        /// </summary>
        void Unbind(Action<object> callback);

        /// <summary>
        /// Unbinds the specified raw value change callback from this object.
        /// </summary>
        void Unbind(Action<object, object> callback);
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
        /// Triggers value changed event with a custom previous value.
        /// </summary>
        void TriggerWithPrevious(T previousValue);

        /// <summary>
        /// Sets the value of the bindable without triggering change event.
        /// </summary>
        void SetWithoutTrigger(T value);

        /// <summary>
        /// Invokes the specified action for modifying the inner value and automatically triggering the bindable.
        /// Affected by TriggerWhenDifferent flag.
        /// </summary>
        void ModifyValue(Action<T> modifyHandler);

        /// <summary>
        /// Makes the value of this bindable bound to the specified bindable.
        /// </summary>
        void BindTo(IBindable<T> other);

        /// <summary>
        /// Makes the value of this bindable no longer bound to the specified bindable.
        /// </summary>
        void UnbindFrom(IBindable<T> other);

        /// <summary>
        /// Binds the specified new value event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<T> callback);

        /// <summary>
        /// Binds the specified value changed event listener and triggers for the listener immediately.
        /// </summary>
        void BindAndTrigger(Action<T, T> callback);

        /// <summary>
        /// Binds the specified new value event listener.
        /// </summary>
        void Bind(Action<T> callback);

        /// <summary>
        /// Binds the specified value changed event listener.
        /// </summary>
        void Bind(Action<T, T> callback);

        /// <summary>
        /// Unbinds the specified new value callback from this object.
        /// </summary>
        void Unbind(Action<T> callback);

        /// <summary>
        /// Unbinds the specified value change callback from this object.
        /// </summary>
        void Unbind(Action<T, T> callback);
    }
}
