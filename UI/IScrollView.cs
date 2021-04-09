using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface IScrollView : IGraphicObject, IHasMask, IHasAlpha {
    
        /// <summary>
        /// Whether the scrollview can be scrolled horizontally.
        /// Default: false.
        /// </summary>
        bool IsHorizontal { get; set; }

        /// <summary>
        /// Whether the scrollview can be scrolled vertically.
        /// Default: true.
        /// </summary>
        bool IsVertical { get; set; }

        /// <summary>
        /// Whether inertia effect should be used.
        /// Default: true
        /// </summary>
        bool UseInertia { get; set; }

        /// <summary>
        /// If supports inertia, how much is the application rate. 
        /// Default: 0.1
        /// </summary>
        float InertiaRate { get; set; }

        /// <summary>
        /// Amount of elasticity applied to scrolling when movement supports elastic mode.
        /// </summary>
        float Elasticity { get; set; }

        /// <summary>
        /// The background sprite of the scrollview.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// The type of movement effect applied on the scrollview.
        /// </summary>
        ScrollRect.MovementType Movement { get; set; }

        /// <summary>
        /// Scrollbar for horizontal scrolling.
        /// </summary>
        IScrollBar HorizontalScrollbar { get; set; }

        /// <summary>
        /// Scrollbar for vertical scrolling.
        /// </summary>
        IScrollBar VerticalScrollbar { get; set; }

        /// <summary>
        /// Returns the viewport object which defines the boundary of the scrollview.
        /// </summary>
        /// <value></value>
        IGraphicObject Viewport { get; }

        /// <summary>
        /// Returns the scrollview content container object.
        /// </summary>
        IGraphicObject Container { get; }


        /// <summary>
        /// Resets the scrollview's position to its origin point.
        /// </summary>
        void ResetPosition();

        /// <summary>
        /// Positions the container to specified position.
        /// </summary>
        void ScrollTo(Vector2 position);
    }
}