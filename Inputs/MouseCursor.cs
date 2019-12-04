using UnityEngine;

namespace PBFramework.Inputs
{
    public class MouseCursor : Cursor {

        private int mouseIndex;


        public MouseCursor(KeyCode keyCode, Vector2 resolution) : base(keyCode, resolution)
        {
            mouseIndex = keyCode - KeyCode.Mouse0;
        }

        /// <summary>
        /// Updates the state of the mouse cursor.
        /// </summary>
        public void Process()
        {
            if(!isActive.Value) return;

            // Refresh cursor state
            if (Input.GetMouseButtonDown(mouseIndex))
                state.Value = InputState.Press;
            else if (Input.GetMouseButtonUp(mouseIndex))
                state.Value = InputState.Release;
            else if (Input.GetMouseButton(mouseIndex))
                state.Value = InputState.Hold;
            else
                state.Value = InputState.Idle;

            // Refresh cursor positions
            var newPos = Input.mousePosition;
            ProcessPosition(newPos.x, newPos.y);
        }
    }
}