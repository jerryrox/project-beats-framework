using UnityEngine;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Represents an input which is either a mouse or a touch pointer.
    /// </summary>
    public interface ICursor {
    
        /// <summary>
        /// Returns the ID of the cursor.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Returns whether this cursor is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Returns the current state of the cursor.
        /// </summary>
        InputState State { get; }

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
    }
}