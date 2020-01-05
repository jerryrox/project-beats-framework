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
    }
}