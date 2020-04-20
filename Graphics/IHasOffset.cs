using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a rect.
    /// </summary>
    public interface IHasOffset {

        /// <summary>
        /// The size offset of the object.
        /// </summary>
        Offset Offset { get; set; }
    }
}