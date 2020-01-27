using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// A basic IProgress extension with minimal features to extend from.
    /// </summary>
    public interface ISimpleProgress : IProgress<float> {

        /// <summary>
        /// Event called when there was an update on the progress.
        /// </summary>
        event Action<float> OnProgress;


        /// <summary>
        /// Returns the progress of the on-going process.
        /// </summary>
        float Progress { get; }
    }
}