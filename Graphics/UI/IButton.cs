using System;
using UnityEngine;

namespace PBFramework.Graphics.UI
{
    public interface IButton : IGraphicObject {

        /// <summary>
        /// Event called when the button has been clicked.
        /// </summary>
        event Action OnClick;
        

        /// <summary>
        /// Returns the label component of the button.
        /// </summary>
        ILabel Label { get; }

        /// <summary>
        /// Returns the background sprite of the button.
        /// </summary>
        ISprite Background { get; }


        /// <summary>
        /// Sets button transition mode to none.
        /// (Default)
        /// </summary>
        void SetNoTransition();

        /// <summary>
        /// Sets button transition mode to sprite swap.
        /// </summary>
        void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled);

        /// <summary>
        /// Sets button transition mode to color tint.
        /// </summary>
        void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration);

        /// <summary>
        /// Sets button transition mode to color tint.
        /// Selected, pressed, disabled colors are adjusted automatically.
        /// </summary>
        void SetColorTintTransition(Color normal, float duration);

    }
}