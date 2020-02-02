using System;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface ITrigger : IGraphicObject {

        /// <summary>
        /// Event called when the mouse pointer has entered this object.
        /// </summary>
        event Action OnPointerEnter;
        
        /// <summary>
        /// Event called when the mouse pointer has exited this object.
        /// </summary>
        event Action OnPointerExit;
        
        /// <summary>
        /// Event called when the mouse pointer has pressed down on this object.
        /// </summary>
        event Action OnPointerDown;
        
        /// <summary>
        /// Event called when the mouse pointer has pressed up on this object.
        /// </summary>
        event Action OnPointerUp;

        /// <summary>
        /// Event called when the mouse pointer has clicked this object.
        /// </summary>
        event Action OnPointerClick;


        /// <summary>
        /// Invaokes the pointer enter event.
        /// </summary>
        void InvokeEnter();

        /// <summary>
        /// Invaokes the pointer exit event.
        /// </summary>
        void InvokeExit();

        /// <summary>
        /// Invaokes the pointer down event.
        /// </summary>
        void InvokeDown();

        /// <summary>
        /// Invaokes the pointer up event.
        /// </summary>
        void InvokeUp();

        /// <summary>
        /// Invaokes the pointer click event.
        /// </summary>
        void InvokeClick();
    }
}