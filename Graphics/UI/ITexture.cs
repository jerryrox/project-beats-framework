﻿using UnityEngine;

namespace PBFramework.Graphics.UI
{
    public interface ITexture : IGraphicObject, IHasColor, IHasMaterial {
    
        /// <summary>
        /// The texture to render.
        /// </summary>
        Texture Texture { get; set; }

        /// <summary>
        /// The UV rect applied on rendering.
        /// </summary>
        Rect UVRect { get; set; }


        /// <summary>
        /// Adjusts the UV rect to fill the RectTransform size while respecting the texture's aspect ratio.
        /// </summary>
        void FillTexture();
    }
}