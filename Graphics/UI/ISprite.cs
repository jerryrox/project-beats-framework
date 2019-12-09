using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public interface ISprite : IHasColor, IHasMaterial, IHasFill {
        
        /// <summary>
        /// The sprite to be displayed on the object.
        /// </summary>
        Sprite Sprite { get; set; }

        /// <summary>
        /// Type of image displaying method.
        /// </summary>
        Image.Type ImageType { get; set; }
    }
}