using UnityEngine;

namespace PBFramework.Graphics.UI
{
    public interface IPanel : IGraphicObject, IHasAlpha, IHasMask {
    
        /// <summary>
        /// The sprite which the masking will be based on.
        /// </summary>
        Sprite MaskSprite { get; set; }
    }
}