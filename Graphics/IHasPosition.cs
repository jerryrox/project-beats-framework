using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a position.
    /// </summary>
    public interface IHasPosition {
    
        /// <summary>
        /// X position of the object.
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Y position of the object.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Z position of the object.
        /// </summary>
        float Z { get; set; }

        /// <summary>
        /// Position of the object in vector.
        /// </summary>
        Vector3 Position { get; set; }
    }
}