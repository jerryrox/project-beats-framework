using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a rect.
    /// </summary>
    public interface IHasRect {
    
        /// <summary>
        /// X position of the rect.
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Y position of the rect.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Width of the rect.
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// Height of the rect.
        /// </summary>
        float Height { get; set; }

        /// <summary>
        /// Position of the rect in vector.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Size of the rect in vector.
        /// </summary>
        Vector2 Size { get; set; }
    }
}