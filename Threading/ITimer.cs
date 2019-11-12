using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// Interface for a timer object.
    /// </summary>
    public interface ITimer : IPromise<ITimer>
    {
        /// <summary>
        /// Event called when the timer has reached its limit.
        /// </summary>
        new event Action<ITimer> OnFinished;

        /// <summary>
        /// Maximum amount of time in seconds which the timer can run for since it started.
        /// Upon reaching the limit, OnFinished event will be fired.
        /// By default, this value is set to float.MaxValue.
        /// </summary>
        float Limit { get; set; }

        /// <summary>
        /// Current elapsed time in seconds.
        /// </summary>
        float Current { get; set; }

        /// <summary>
        /// Returns whether the timer is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// A specifiable progress listener if desired.
        /// </summary>
        IProgress<float> Progress { get; set; }


        /// <summary>
        /// Pauses the timer.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        void Stop();
    }
}