using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Fonts;

namespace PBFramework.Graphics.UI
{
    public interface ILabel : IGraphicObject, IHasColor {
        
        /// <summary>
        /// The font this label will use for rendering the text.
        /// </summary>
        IFont Font { get; set; }

        /// <summary>
        /// Whether the text should be rendered using a bold font.
        /// Default: false
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Whether the text should be displayed using italics.
        /// Default: false
        /// </summary>
        bool IsItalic { get; set; }

        /// <summary>
        /// Whether the text should be wrapped within the size width and height.
        /// Default: false.
        /// </summary>
        bool WrapText { get; set; }

        /// <summary>
        /// Size of the displayed text.
        /// Default: 20
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// The pivot point which the text will be aligned from.
        /// Default: UpperLeft
        /// </summary>
        TextAnchor Alignment { get; set; }

        /// <summary>
        /// The actual text value to be displayed on the label.
        /// </summary>
        string Text { get; set; }
    }
}