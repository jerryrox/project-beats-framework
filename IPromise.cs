using System;
using PBFramework.Threading;

namespace PBFramework
{
    /// <summary>
    /// An interface used to generalize a long-running process which may be running (a)synchronously but its
    /// completion state and result must be known at some point.
    /// </summary>
    public interface IPromise : IHasProgress {

        /// <summary>
        /// Event called from underlying process when it has finished.
        /// </summary>
        event Action OnFinished;

        /// <summary>
        /// The result evaluated from underlying process.
        /// </summary>
        object RawResult { get; }

        /// <summary>
        /// Returns whether the underlying process is finished.
        /// </summary>
        bool IsFinished { get; }
    }

    /// <summary>
    /// A generic IPromise extension for specific Result of type T.
    /// </summary>
    public interface IPromise<T> : IPromise
    {
        /// <summary>
        /// Event called from underlying process when it has finished.
        /// </summary>
        event Action<T> OnFinishedResult;

        /// <summary>
        /// The result evaluated from underlying process.
        /// </summary>
        T Result { get; }
    }
}