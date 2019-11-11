using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// A basic IProgress extension with minimal features to extend from.
    /// </summary>
    public interface ISimpleProgress : IProgress<float> {
    
        /// <summary>
        /// Returns the progress of the on-going process.
        /// </summary>
        float Progress { get; }
    }
}