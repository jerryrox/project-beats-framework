using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    public class TouchCursor : Cursor {

        /// <summary>
        /// Unique identifier of the update call passed from InputManager.
        /// Used for indication of update flag on current frame.
        /// </summary>
        private uint updateId;


        public TouchCursor(KeyCode keyCode, Vector2 resolution) : base(keyCode, resolution) {}

        public override void SetActive(bool active)
        {
            // Activation of touch shouldn't occur just because the active flag is assigned true.
            // It must only be active when the touch has entered the Press state.
            // Therefore, this method should only handle case for active == false.
            if (!active)
            {
                state.Value = InputState.Idle;
                isActive.Value = active;
            }
        }

        public void Process(Touch touch, uint updateId)
        {
            if(!isActive.Value) return;

            // Store update id
            this.updateId = updateId;

            // Refresh touch state
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Activate touch.
                    isActive.Value = true;

                    state.Value = InputState.Press;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    state.Value = InputState.Release;
                    break;
                default:
                    state.Value = InputState.Hold;
                    break;
            }

            // Process position
            Vector2 newPos = touch.position;
            ProcessPosition(newPos.x, newPos.y);
        }

        public override void Release() => SetActive(false);

        /// <summary>
        /// Verifies whether the cursor has been updated this frame.
        /// If not, the cursor should be on Idle state.
        /// </summary>
        public void VerifyTouch(uint targetId)
        {
            // If already idle or has been updated this frame, just return.
            if(state.Value == InputState.Idle || updateId == targetId)
                return;
            // Deactive the touch through release.
            Release();
        }
    }
}