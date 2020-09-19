using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Testing
{
    /// <summary>
    /// An object representing an action which can be invoked manually or automatically within the test environment.
    /// </summary>
    public class TestAction {

        /// <summary>
        /// Delegate for handling key actions.
        /// Specifies whether the call is from automatic testing process.
        /// </summary>
        public delegate IEnumerator ActionHandler(bool isAuto);

        private bool hasKey = false;


        /// <summary>
        /// Types of events that can trigger the action.
        /// </summary>
        public ActionTrigger Trigger { get; private set; } = ActionTrigger.Both;

        /// <summary>
        /// Key binding for executing this action in manual testing mode.
        /// </summary>
        public KeyCode Key { get; private set; }

        /// <summary>
        /// The actual action handler.
        /// </summary>
        public ActionHandler Handler { get; private set; }

        /// <summary>
        /// The description of the action.
        /// </summary>
        public string Description { get; private set; }


        public TestAction(Func<IEnumerator> action, string description = null) :
            this(isAuto => action?.Invoke(), description) { }

        public TestAction(ActionHandler action, string description = null)
        {
            this.Trigger = ActionTrigger.Auto;
            this.Handler = action;
            this.Description = description ?? "";
        }

        public TestAction(KeyCode keyCode, Func<IEnumerator> action, string description) :
            this(false, keyCode, isAuto => action?.Invoke(), description) {}

        public TestAction(bool manualOnly, KeyCode keyCode, Func<IEnumerator> action, string description) :
            this(manualOnly, keyCode, isAuto => action?.Invoke(), description) {}

        public TestAction(bool manualOnly, KeyCode keyCode, ActionHandler action, string description)
        {
            hasKey = true;

            this.Trigger = manualOnly ? ActionTrigger.Manual : ActionTrigger.Both;
            this.Key = keyCode;
            this.Handler = action;
            this.Description = description;
        }

        /// <summary>
        /// Returns the displayed usage description for the bound key.
        /// </summary>
        public string GetUsage()
        {
            if(hasKey)
                return $"[KeyCode({Key})] : {Description}";
            return Description;
        }

        /// <summary>
        /// Checks whether the bound key is pressed and if true, execute the associated action.
        /// </summary>
        public IEnumerator RunAction(bool isManual)
        {
            if(Handler == null)
                return null;
            if((!isManual && Trigger == ActionTrigger.Manual) ||
                (isManual && Trigger == ActionTrigger.Auto))
                return null;
            if(isManual && !Input.GetKeyDown(Key))
                return null;
            return Handler.Invoke(!isManual);
        }
    }
}