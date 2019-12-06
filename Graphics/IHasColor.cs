using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a color property.
    /// </summary>
    public interface IHasColor {
    
        /// <summary>
        /// Color of the object.
        /// </summary>
        Color Color { get; set; }
    }
}