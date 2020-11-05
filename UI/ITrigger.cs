using System;
using PBFramework.Inputs;
using PBFramework.Graphics;
using UnityEngine.EventSystems;

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
        /// Event called when the trigger is about to be dragged on.
        /// </summary>
        event Action<ICursor> OnDragStart;

        /// <summary>
        /// Event called when the trigger is currently being dragged.
        /// </summary>
        event Action<ICursor> OnDragging;

        /// <summary>
        /// Event called when the trigger is about to end dragging.
        /// </summary>
        event Action<ICursor> OnDragEnd;


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