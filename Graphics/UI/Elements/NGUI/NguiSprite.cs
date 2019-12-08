using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics.UI.Elements.NGUI
{
    /// <summary>
    /// ISprite implementation using NGUI.
    /// </summary>
    public class NguiSprite : NguiElement<UISprite>, ISprite {

        public int Depth
        {
            get => component.depth;
            set => component.depth = value;
        }

        public bool UseGradient
        {
            get => component.applyGradient;
            set => component.applyGradient = value;
        }

        public Color TopGradient
        {
            get => component.gradientTop;
            set => component.gradientTop = value;
        }

        public Color BottomGradient
        {
            get => component.gradientBottom;
            set => component.gradientBottom = value;
        }

        public Color Color
        {
            get => component.color;
            set => component.color = value;
        }

        public float Alpha
        {
            get => component.alpha;
            set => component.alpha = value;
        }

        public float Width
        {
            get => component.width;
            set => component.width = (int)value;
        }

        public float Height
        {
            get => component.height;
            set => component.height = (int)value;
        }

        public Pivots Pivot
        {
            get => NguiGraphicsHelper.NguiToGraphicsPivot(component.pivot);
            set => component.pivot = NguiGraphicsHelper.GraphicsToNguiPivot(value);
        }

        public string Spritename
        {
            get => component.spriteName;
            set => component.spriteName = value;
        }


        protected override void Awake()
        {
            base.Awake();
            UseGradient = false;
        }

        public void ResetSize()
        {
            component.MakePixelPerfect();
        }
    }
}