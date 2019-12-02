using UnityEngine;

namespace PBFramework.Inputs
{
    public class MouseCursor : Cursor {

        public MouseCursor(uint id, Vector2 resolution) : base(id, resolution) {}

        /// <summary>
        /// Updates the state of the mouse cursor.
        /// </summary>
        public void Process()
        {
            // Refresh cursor state
            if (Input.GetMouseButtonDown(id))
                State = InputState.Press;
            else if (Input.GetMouseButtonUp(id))
                State = InputState.Release;
            else if (Input.GetMouseButton(id))
                State = InputState.Hold;
            else
                State = InputState.Idle;

            // Refresh cursor positions
            var newPos = Input.mousePosition;
            ProcessPosition(newPos.x, newPos.y);
        }
    }
}