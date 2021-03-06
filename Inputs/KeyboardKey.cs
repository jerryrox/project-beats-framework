using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Data.Bindables;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Default implementation of IKey for keyboard inputs.
    /// </summary>
    public class KeyboardKey : IKey {

        private KeyCode keyCode;

        private Bindable<InputState> state;

        private BindableBool isActive;


        public KeyCode Key => keyCode;

        public IReadOnlyBindable<InputState> State => state;

        public IReadOnlyBindable<bool> IsActive => isActive;

        /// <summary>
        /// The current number of objects listening to this key.
        /// </summary>
        public int Listeners { get; set; } = 1;


        public KeyboardKey(KeyCode keyCode)
        {
            this.keyCode = keyCode;

            this.state = new Bindable<InputState>()
            {
                TriggerWhenDifferent = true
            };
            this.isActive = new BindableBool()
            {
                TriggerWhenDifferent = true
            };
        }

        public void Release() => state.Value = InputState.Idle;

        public void SetActive(bool active) => isActive.Value = active;

        public void Process()
        {
            if(!isActive.Value) return;

            // Process input state
            if(Input.GetKeyDown(keyCode))
                state.Value = InputState.Press;
            else if(Input.GetKeyUp(keyCode))
                state.Value = InputState.Release;
            else if(Input.GetKey(keyCode))
                state.Value = InputState.Hold;
            else
                state.Value = InputState.Idle;
        }
    }
}