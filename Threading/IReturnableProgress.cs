using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// IProgress extension which allows the target process to return a value through a property.
    /// </summary>
    public interface IReturnableProgress : ISimpleProgress, IEventProgress
    {
        /// <summary>
        /// Value returned from the target process.
        /// </summary>
        object Value { get; set; }


        /// <summary>
        /// Invokes OnFinished event.
        /// </summary>
        void InvokeFinished(object value);
    }

    public interface IReturnableProgress<T> : IReturnableProgress
    {
        /// <summary>
        /// Events called when the process has been finished.
        /// </summary>
        new event Action<T> OnFinished;


        /// <summary>
        /// Value returned from the target process.
        /// </summary>
        new T Value { get; set; }

        /// <summary>
        /// Invokes OnFinished event.
        /// </summary>
        void InvokeFinished(T value);
    }
}