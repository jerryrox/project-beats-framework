using UnityEngine;
using PBFramework.Assets.Fonts;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface ILabel : IGraphicObject, IHasColor, IHasTint, IRaycastable {

        /// <summary>
        /// The preferred width of the label for current text and the label settings.
        /// </summary>
        float PreferredWidth { get; }

        /// <summary>
        /// The preferred height of the label for current text and the label settings.
        /// </summary>
        float PreferredHeight { get; }

        /// <summary>
        /// The preferred size of the label for current text and label settings.
        /// </summary>
        Vector2 PreferredSize { get; }

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
        /// Default: MiddleCenter
        /// </summary>
        TextAnchor Alignment { get; set; }

        /// <summary>
        /// The actual text value to be displayed on the label.
        /// </summary>
        string Text { get; set; }
    }
}