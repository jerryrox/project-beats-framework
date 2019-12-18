using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a size.
    /// </summary>
    public interface IHasSize {
    
        /// <summary>
        /// Width of the object.
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// Height of the object.
        /// </summary>
        float Height { get; set; }

        /// <summary>
        /// Size of the object in vector.
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Resets the size of the element using this rect.
        /// </summary>
        void ResetSize();
    }
}