using System;
using UnityEngine.UI;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface ISlider : IGraphicObject, IHasTransition {

        /// <summary>
        /// Event called when the scrollbar value has changed.
        /// </summary>
        event Action<float> OnChange;


        /// <summary>
        /// Returns the background sprite of the slider.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// Returns the foreground sprite of the slider.
        /// </summary>
        ISprite Foreground { get; }

        /// <summary>
        /// Returns the thumb sprite of the slider.
        /// </summary>
        ISprite Thumb { get; }

        /// <summary>
        /// The direction of the slider movement.
        /// Default: LeftToRight
        /// </summary>
        Slider.Direction Direction { get; set; }

        /// <summary>
        /// Whether the slider value should move on whole numbers only.
        /// </summary>
        bool IsWholeNumber { get; set; }

        /// <summary>
        /// The minimum value of the slider scale.
        /// </summary>
        float MinValue { get; set; }

        /// <summary>
        /// The maximum value of the slider scale.
        /// </summary>
        float MaxValue { get; set; }

        /// <summary>
        /// Current slider value.
        /// </summary>
        float Value { get; set; }
    }
}