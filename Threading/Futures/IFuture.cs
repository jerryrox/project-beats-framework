using System;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// Interface of a task which is expected to run asynchronously.
    /// It must be assumed that each Future instance can only be used a single time.
    /// Methods should return IFuture or its typed variant if the inner task will automatically run upon returning this instance.
    /// </summary>
    public interface IFuture : IDisposable {

        /// <summary>
        /// Returns the progress of the inner task.
        /// </summary>
        IReadOnlyBindable<float> Progress { get; }

        /// <summary>
        /// Returns whether this object has been disposed.
        /// </summary>
        IReadOnlyBindable<bool> IsDisposed { get; }

        /// <summary>
        /// Returns whether the inner task has completed its task.
        /// </summary>
        IReadOnlyBindable<bool> IsCompleted { get; }

        /// <summary>
        /// Returns the error thrown from the inner task, if exists.
        /// </summary>
        IReadOnlyBindable<Exception> Error { get; }

        /// <summary>
        /// Whether all the state changes and event invocations must be done in the Unity's main thread.
        /// If false, there is no guarantee the events will be fired in either Unity/non-Unity threads.
        /// Default: true
        /// </summary>
        bool IsThreadSafe { get; set; }

        /// <summary>
        /// Returns whether the progress has been run once.
        /// </summary>
        bool DidRun { get; }
    }

    /// <summary>
    /// A generic version of IFuture which comes with an output object of given type.
    /// </summary>
    public interface IFuture<T> : IFuture
    {
        /// <summary>
        /// Returns the evaluated output instance given from the inner task, if exists.
        /// </summary>
        IReadOnlyBindable<T> Output { get; }
    }
}
