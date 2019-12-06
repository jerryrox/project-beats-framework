using System;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Indicates that the element can be interacted using cursor.
    /// </summary>
    public interface ICursorInteractible : IElement, IHasSize {

        /// <summary>
        /// Event called when the object has been hovered on with a cursor.
        /// </summary>
        event Action<ICursorInteractible> OnOver;

        /// <summary>
        /// Event called when the object has been hovered out with a cursor.
        /// </summary>
        event Action<ICursorInteractible> OnOut;

        /// <summary>
        /// Event called when the object has been pressed with a cursor.
        /// </summary>
        event Action<ICursorInteractible> OnPress;

        /// <summary>
        /// Event called when object has been released with a cursor.
        /// </summary>
        event Action<ICursorInteractible> OnRelease;

        /// <summary>
        /// Event called when object has been clicked with a cursor.
        /// </summary>
        event Action<ICursorInteractible> OnClick;


        /// <summary>
        /// Triggers over event manually.
        /// </summary>
        void TriggerOver();

        /// <summary>
        /// Triggers out event manually.
        /// </summary>
        void TriggerOut();

        /// <summary>
        /// Triggers press event manually.
        /// </summary>
        void TriggerPress();

        /// <summary>
        /// Triggers release event manually.
        /// </summary>
        void TriggerRelease();

        /// <summary>
        /// Triggers click event manually.
        /// </summary>
        void TriggerClick();

        /// <summary>
        /// Binds a target with size 
        /// </summary>
        void BindSize<T>(T target) where T : IHasSize, IComponent;
    }
}