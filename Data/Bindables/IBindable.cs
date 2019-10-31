using System;

namespace PBFramework.Data.Bindables
{
    public interface IBindable
    {
        /// <summary>
        /// Events called when the inner value has changed.
        /// Passes an Object type of the value as parameter.
        /// </summary>
        event Action<object> OnRawValueChanged;


        /// <summary>
        /// The inner value stored in Object type.
        /// </summary>
        object RawValue { get; set; }


        /// <summary>
        /// Triggers value changed event.
        /// </summary>
        void Trigger();

        /// <summary>
        /// Parses the specified string value into the type that matches the bound type.
        /// </summary>
        void Parse(string value);
    }

    public interface IBindable<T> : IBindable
    {
        /// <summary>
        /// Events called when the inner value has changed.
        /// </summary>
        event Action<T> OnValueChanged;


        /// <summary>
        /// The inner value stored.
        /// </summary>
        T Value { get; set; }
    }
}
