using UnityEngine;
using PBFramework.Assets.Fonts;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface IDropdownProperty : IHasTransition {

        /// <summary>
        /// The max height of the dropdown popup before supporting scrollbar.
        /// </summary>
        float MaxHeight { get; set; }

        /// <summary>
        /// The height of each dropdown entry.
        /// Default: 24
        /// </summary>
        float EntryHeight { get; set; }

        /// <summary>
        /// Returns the background sprite of the dropdown popup.
        /// </summary>
        ISprite PopupBackground { get; }

        /// <summary>
        /// Returns the scrollbar of the dropdown popup.
        /// </summary>
        IScrollBar PopupScrollbar { get; }

        /// <summary>
        /// Size of the font for each dropdown entry.
        /// Default: 20
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// Font family to use for each dropdown entry label.
        /// </summary>
        IFont Font { get; set; }

        /// <summary>
        /// Color of each dropdown entry label.
        /// Default: (0.25, 0.25, 0.25, 1)
        /// </summary>
        Color LabelColor { get; set; }

        /// <summary>
        /// Color of the dropdown entry background when it's currently selected.
        /// Default: (0.8, 0.8, 0.8, 1)
        /// </summary>
        Color SelectedColor { get; set; }
    }
}