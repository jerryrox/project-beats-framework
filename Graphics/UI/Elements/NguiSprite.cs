using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// ISprite implementation using NGUI.
    /// </summary>
    public class NguiSprite : NguiElement<UISprite>, ISprite {

        protected UISprite sprite;
    
        
        public int Depth
        {
            get => sprite.depth;
            set => sprite.depth = value;
        }

        public Color Color
        {
            get => sprite.color;
            set => sprite.color = value;
        }

        public float Alpha
        {
            get => sprite.alpha;
            set => sprite.alpha = value;
        }

        public float Width
        {
            get => sprite.width;
            set => sprite.width = (int)value;
        }

        public float Height
        {
            get => sprite.height;
            set => sprite.height = (int)value;
        }

        public Pivots Pivot
        {
            get => NguiGraphicsHelper.NguiToGraphicsPivot(sprite.pivot);
            set => sprite.pivot = NguiGraphicsHelper.GraphicsToNguiPivot(value);
        }

        public string Spritename
        {
            get => sprite.spriteName;
            set => sprite.spriteName = value;
        }


        public void ResetSize()
        {
            sprite.MakePixelPerfect();
        }
    }
}