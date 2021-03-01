using UnityEngine;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Represents an input which is either a mouse or a touch pointer.
    /// </summary>
    public interface ICursor : IInput {
    
        /// <summary>
        /// Returns the raw position of the cursor.
        /// </summary>
        Vector2 RawPosition { get; }

        /// <summary>
        /// Returns the raw delta position of the cursor since last frame.
        /// </summary>
        Vector2 RawDelta { get; }

        /// <summary>
        /// Returns the processed position of the cursor.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Returns the processed delta position of the cursor since last frame.
        /// </summary>
        Vector2 Delta { get; }


        /// <summary>
        /// Adjusts internal state to work for the specified screen resolution.
        /// </summary>
        void SetResolution(Vector2 resolution);
    }
}