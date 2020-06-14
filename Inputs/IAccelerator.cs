using UnityEngine;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Interface of an accelerometer provider.
    /// </summary>
    public interface IAccelerator {
    
        /// <summary>
        /// Returns the acceleration value.
        /// </summary>
        Vector2 Acceleration { get; }


        /// <summary>
        /// Updates accelerator to capture data.
        /// </summary>
        void Update();
    }
}