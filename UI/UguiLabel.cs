using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Fonts;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBFramework.UI
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

        public Color Tint
        {
            get => component.color;
            set
            {
                value.a = component.color.a;
                component.color = value;
            }
        }

        public float PreferredWidth => component.preferredWidth;

        public float PreferredHeight => component.preferredHeight;

        public Vector2 PreferredSize => new Vector2(component.preferredWidth, component.preferredHeight);

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

        public bool IsRaycastTarget
        {
            get => component.raycastTarget;
            set => component.raycastTarget = value;
        }


        protected override void Awake()
        {
            base.Awake();
            FontSize = 20;
            Alignment = TextAnchor.MiddleCenter;
            WrapText = false;
            this.Color = Color.white;
        }

        [InitWithDependency]
        private void Init(IFontManager fontManager)
        {
            Font = fontManager.DefaultFont.Value;
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
                if(font.HasBoldItalic)
                    component.fontStyle = FontStyle.Normal;
                else if(font.HasBold)
                    component.fontStyle = FontStyle.Italic;
                else if (font.HasItalic)
                    component.fontStyle = FontStyle.Bold;
                else
                    component.fontStyle = FontStyle.BoldAndItalic;
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
                component.resizeTextForBestFit = true;
            }
            else
            {
                component.horizontalOverflow = HorizontalWrapMode.Overflow;
                component.verticalOverflow = VerticalWrapMode.Overflow;
                component.resizeTextForBestFit = false;
            }
        }
    }
}