using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Testing
{
    /// <summary>
    /// An object representing a keybinding action for use in manual testing.
    /// </summary>
    public class TestKeyBinding {

        private KeyCode keyCode;
        private Func<IEnumerator> action;
        private string description;


        /// <summary>
        /// Whether the test action bound to this object must be executed manually.
        /// If possible, it is recommended to make tests automatable.
        /// </summary>
        public bool ForceManual { get; set; } = false;


        public TestKeyBinding(KeyCode keyCode, Func<IEnumerator> action, string description)
        {
            this.keyCode = keyCode;
            this.action = action;
            this.description = description;
        }

        /// <summary>
        /// Returns the displayed usage description for the bound key.
        /// </summary>
        public string GetUsage() => $"[KeyCode({keyCode})] : {description}";

        /// <summary>
        /// Checks whether the bound key is pressed and if true, execute the associated action.
        /// </summary>
        public IEnumerator RunAction(bool isManual)
        {
            if(action == null)
                return null;
            if(!isManual && ForceManual)
                return null;
            if(isManual && !Input.GetKeyDown(keyCode))
                return null;
            return action.Invoke();
        }
    }
}