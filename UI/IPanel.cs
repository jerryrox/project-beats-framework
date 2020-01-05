using UnityEngine;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface IPanel : IGraphicObject, IHasAlpha, IHasMask {
    
        /// <summary>
        /// The sprite which the masking will be based on.
        /// </summary>
        Sprite MaskSprite { get; set; }
    }
}