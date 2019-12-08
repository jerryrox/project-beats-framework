using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics.UI.Elements.NGUI
{
    public class NguiLabel : NguiElement<UILabel>, ILabel {
    
    
        public int Depth
        {
            get => component.depth;
            set => component.depth = value;
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

        public string Text
        {
            get => component.text;
            set => component.text = value;
        }

        public bool UseEllipsis
        {
            get => component.overflowEllipsis;
            set => component.overflowEllipsis = value;
        }

        public Alignments Alignment
        {
            get => NguiGraphicsHelper.NguiToGraphicsAlignment(component.alignment);
            set => component.alignment = NguiGraphicsHelper.GraphicsToNguiAlignment(value);
        }

        public SizeWrapModes TextWrap
        {
            get => NguiGraphicsHelper.NguiToGraphicsOverflow(component.overflowMethod);
            set => component.overflowMethod = NguiGraphicsHelper.GraphicsToNguiOverflow(value);
        }


        protected override void Awake()
        {
            base.Awake();
            UseGradient = false;
        }

        public void ResetSize()
        {
            // component
            component.MakePixelPerfect();
        }
    }
}