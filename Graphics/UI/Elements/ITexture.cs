using UnityEngine;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a 2D texture.
    /// </summary>
    public interface ITexture : IElement, IHasSize, IHasColor, IHasDepth {
    
        /// <summary>
        /// Texture to be displayed.
        /// </summary>
        Texture2D Texture { get; set; }

        
    }
}