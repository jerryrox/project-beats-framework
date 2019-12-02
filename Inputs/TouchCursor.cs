using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    public class TouchCursor : Cursor {
    
        public TouchCursor(uint id, Vector2 resolution) : base(id, resolution) {}

        public void SetActive(bool active) => IsActive = active;

        public void Process(Touch touch)
        {
            // Refresh touch state
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    State = InputState.Press;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    State = InputState.Release;
                    break;
                default:
                    State = InputState.Hold;
                    break;
            }

            // Process position
            Vector2 newPos = touch.position;
            ProcessPosition(newPos.x, newPos.y);
        }
    }
}