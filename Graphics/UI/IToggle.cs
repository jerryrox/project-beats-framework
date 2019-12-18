using System;

namespace PBFramework.Graphics.UI
{
    public interface IToggle : IGraphicObject, IHasAlpha, IHasTransition {

        /// <summary>
        /// Event called when the toggle value has been changed.
        /// </summary>
        event Action<bool> OnChange;


        /// <summary>
        /// Whether fading transition should occur on the tick.
        /// </summary>
        bool UseFade { get; set; }

        /// <summary>
        /// Whether the toggle is currently ticked.
        /// Default: false
        /// </summary>
        bool Value { get; set; }

        /// <summary>
        /// Returns the tick background sprite.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// Returns the sprite which displays the tick of the toggle.
        /// </summary>
        ISprite Tick { get; }

        /// <summary>
        /// Returns the label on the toggle.
        /// </summary>
        ILabel Label { get; }
    }
}