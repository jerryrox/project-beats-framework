using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    public interface IInputManager {

        /// <summary>
        /// Returns the number of mouse cursors being managed.
        /// </summary>
        int MaxMouseCount { get; }

        /// <summary>
        /// Returns the number of touch cursors being managed.
        /// </summary>
        int MaxTouchCount { get; }

        /// <summary>
        /// The accelerator module to use to capture acceleration information.
        /// </summary>
        IAccelerator Accelerator { get; set; }

        /// <summary>
        /// Whether the mouse input will be processed by the manager.
        /// </summary>
        bool UseMouse { get; set; }

        /// <summary>
        /// Whether the touch input will be processed by the manager.
        /// </summary>
        bool UseTouch { get; set; }

        /// <summary>
        /// Whether the keyboard input will be processed by the manager.
        /// </summary>
        bool UseKeyboard { get; set; }

        /// <summary>
        /// Whether acceleration should be captured.
        /// </summary>
        bool UseAcceleration { get; set; }


        /// <summary>
        /// Adds the specified input receiver to layered input handling queue.
        /// </summary>
        void AddReceiver(IInputReceiver receiver);

        /// <summary>
        /// Removes the specified input receiver from layered input handling queue.
        /// </summary>
        void RemoveReceiver(IInputReceiver receiver);

        /// <summary>
        /// Adds the specified keycode to be managed by this object.
        /// </summary>
        IKey AddKey(KeyCode keyCode);

        /// <summary>
        /// Removes the key associated with the specified keycode.
        /// </summary>
        void RemoveKey(KeyCode keyCode);

        /// <summary>
        /// Returns the specific mouse type cursor for specified index.
        /// </summary>
        ICursor GetMouse(int index);

        /// <summary>
        /// Returns the specific touch type cursor for specified index.
        /// </summary>
        ICursor GetTouch(int index);

        /// <summary>
        /// Returns the specific key input for specified keycode.
        /// </summary>
        IKey GetKey(KeyCode keyCode);

        /// <summary>
        /// Returns all mouse currently being managed.
        /// </summary>
        IEnumerable<ICursor> GetMouses();

        /// <summary>
        /// Returns all touches currently being managed.
        /// </summary>
        IEnumerable<ICursor> GetTouches();

        /// <summary>
        /// Returns all keys currently being managed.
        /// </summary>
        IEnumerable<IKey> GetKeys();
    }
}