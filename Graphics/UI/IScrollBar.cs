using System;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public interface IScrollBar : IGraphicObject, IHasTransition {

        /// <summary>
        /// Event called when the scrollbar value has changed.
        /// </summary>
        event Action<float> OnChange;


        /// <summary>
        /// Returns the background sprite of the scrollbar.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// Returns the foreground sprite of the scrollbar.
        /// </summary>
        ISprite Foreground { get; }

        /// <summary>
        /// Scrolling direction of the scrollbar.
        /// Default: LeftToRight
        /// </summary>
        Scrollbar.Direction Direction { get; set; }

        /// <summary>
        /// Progress value of scrollbar
        /// </summary>
        float Value { get; set; }

        /// <summary>
        /// Size of the foreground relative to the background.
        /// </summary>
        float ForegroundSize { get; set; }

        /// <summary>
        /// Number of sliding steps.
        /// </summary>
        int Steps { get; set; }
    }
}