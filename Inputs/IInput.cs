using System;
using UnityEngine;
using PBFramework.Data.Bindables;

namespace PBFramework.Inputs
{
    public interface IInput {

        /// <summary>
        /// Returns the keycode which is represented by this input.
        /// </summary>
        KeyCode Key { get; }

        /// <summary>
        /// Returns the bindable state of the input.
        /// </summary>
        IReadOnlyBindable<InputState> State { get; }
        
        /// <summary>
        /// Returns the bindable active state of the input.
        /// </summary>
        IReadOnlyBindable<bool> IsActive { get; }


        /// <summary>
        /// Forcefully releases the input.
        /// </summary>
        void Release();

        /// <summary>
        /// Sets whether the input is active.
        /// </summary>
        void SetActive(bool active);
    }
}