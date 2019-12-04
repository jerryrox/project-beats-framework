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
        /// Returns all keys currently being managed.
        /// </summary>
        IEnumerable<IKey> GetKeys();

        /// <summary>
        /// Sets whether mouse cursor will be processed by the manager.
        /// </summary>
        void UseMouse(bool use);

        /// <summary>
        /// Sets whether touch cursor will be processed by the manager.
        /// </summary>
        void UseTouch(bool use);

        /// <summary>
        /// Sets whether keyboard 
        /// </summary>
        void UseKeyboard(bool use);
    }
}