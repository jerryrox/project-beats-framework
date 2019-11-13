using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// Interface which indicates that the object has a progress value and can propagate progress events when an update occurs.
    /// </summary>
    public interface IHasProgress {

        /// <summary>
        /// Event called when there is an update on the progress.
        /// </summary>
        event Action<float> OnProgress;

    
        /// <summary>
        /// Current progress of the underlying process.
        /// </summary>
        float Progress { get; }
    }
}