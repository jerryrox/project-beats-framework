using UnityEngine;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Indicates that the element may be masked.
    /// </summary>
    public interface IHasMask {
    
        /// <summary>
        /// The texture used for masking the element.
        /// </summary>
        Texture2D Mask { get; set; }
    }
}