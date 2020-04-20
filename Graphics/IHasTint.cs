using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be tinted.
    /// You can implement this interface instead of IHasColor if changing Alpha isn't required or if the use case semantically doesn't make sense for it.
    /// </summary>
    public interface IHasTint {
        
        /// <summary>
        /// The color tinted on the object or an inner element.
        /// </summary>
        Color Tint { get; set; }
    }
}