using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// Interface for a timer object.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Event called when the timer has finished running.
        /// </summary>
        event Action OnFinished;

        /// <summary>
        /// Event called when the timer has new progress.
        /// </summary>
        event Action<float> OnProgress;


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
        /// Returns the current progress of the timer.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Returns whether the timer is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Returns whether the timer has finished running.
        /// </summary>
        bool IsFinished { get; }


        /// <summary>
        /// Pauses the timer.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts or resumes the timer.
        /// </summary>
        void Start();
    }
}