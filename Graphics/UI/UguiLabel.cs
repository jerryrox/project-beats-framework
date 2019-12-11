using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Fonts;

namespace PBFramework.Graphics.UI
{
    public class UguiLabel : UguiObject<Text>, ILabel {

        private bool isBold;
        private bool isItalic;
        private bool wrapText;

        private IFont font;


        public float Alpha
        {
            get => component.color.a;
            set => component.SetAlpha(value);
        }

        public Color Color
        {
            get => component.color;
            set => component.color = value;
        }

        public IFont Font
        {
            get => font;
            set
            {
                font = value;
                RefreshFontStyle();
            }
        }

        public bool IsBold
        {
            get => isBold;
            set
            {
                isBold = value;
                RefreshFontStyle();
            }
        }

        public bool IsItalic
        {
            get => isItalic;
            set
            {
                isItalic = value;
                RefreshFontStyle();
            }
        }

        public bool WrapText
        {
            get => wrapText;
            set
            {
                wrapText = value;
                RefreshWrapMode();
            }
        }

        public int FontSize
        {
            get => component.fontSize;
            set => component.fontSize = component.resizeTextMaxSize = value;
        }

        public TextAnchor Alignment
        {
            get => component.alignment;
            set => component.alignment = value;
        }

        public string Text
        {
            get => component.text;
            set => component.text = value;
        }


        protected override void Awake()
        {
            base.Awake();
            FontSize = 20;
            Alignment = TextAnchor.UpperLeft;
            WrapText = false;
            this.Color = Color.white;
        }

        /// <summary>
        /// Resets the state of the component font style based on current property values.
        /// </summary>
        private void RefreshFontStyle()
        {
            if(font == null)
                return;
                
            if (isBold && isItalic)
            {
                component.font = font.BoldItalic;
                component.fontStyle = font.HasBoldItalic ? FontStyle.Normal : FontStyle.BoldAndItalic;
            }
            else if (isBold)
            {
                component.font = font.Bold;
                component.fontStyle = font.HasBold ? FontStyle.Normal : FontStyle.Bold;
            }
            else if (isItalic)
            {
                component.font = font.Italic;
                component.fontStyle = font.HasItalic ? FontStyle.Normal : FontStyle.Italic;
            }
            else
            {
                component.font = font.Normal;
                component.fontStyle = FontStyle.Normal;
            }
        }

        /// <summary>
        /// Resets the state of the component wrap mode based on current property values.
        /// </summary>
        private void RefreshWrapMode()
        {
            if (wrapText)
            {
                component.horizontalOverflow = HorizontalWrapMode.Wrap;
                component.verticalOverflow = VerticalWrapMode.Truncate;
            }
            else
            {
                component.horizontalOverflow = HorizontalWrapMode.Overflow;
                component.verticalOverflow = VerticalWrapMode.Overflow;
            }
        }
    }
}