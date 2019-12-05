using UnityEngine;
using PBFramework.Data.Bindables;

namespace PBFramework.Inputs
{
    public abstract class Cursor : ICursor {

        protected readonly KeyCode keyCode;

        /// <summary>
        /// Conversion scale applied while converting raw vector to processed vector.
        /// </summary>
        protected readonly Vector2 processScale;

        /// <summary>
        /// Resolution of the game.
        /// </summary>
        protected readonly Vector2 resolution;

        protected readonly Bindable<InputState> state;
        protected readonly BindableBool isActive;

        protected Vector2 rawPosition = new Vector2();
        protected Vector2 rawDelta = new Vector2();
        protected Vector2 position = new Vector2();
        protected Vector2 delta = new Vector2();


        public KeyCode Key => keyCode;

        public IReadOnlyBindable<InputState> State => state;

        public IReadOnlyBindable<bool> IsActive => isActive;

        public Vector2 RawPosition => rawPosition;

        public Vector2 RawDelta => rawDelta;

        public Vector2 Position => position;

        public Vector2 Delta => delta;


        protected Cursor(KeyCode keyCode, Vector2 resolution)
        {
            this.keyCode = keyCode;
            this.resolution = resolution;

            this.state = new Bindable<InputState>(InputState.Idle)
            {
                TriggerWhenDifferent = true
            };
            this.isActive = new BindableBool(false)
            {
                TriggerWhenDifferent = true
            };

            processScale = new Vector2(1f / Screen.width, 1f / Screen.height);
        }

        public virtual void SetActive(bool active) => isActive.Value = active;

        public virtual void Release() => state.Value = InputState.Idle;

        /// <summary>
        /// Internally processes cursor position, including raw and non-raw positions.
        /// </summary>
        protected void ProcessPosition(float newX, float newY)
        {
            rawDelta.x = newX - rawPosition.x;
            rawDelta.y = newY - rawPosition.y;
            rawPosition.x = newX;
            rawPosition.y = newY;

            float newProcessedX = (rawPosition.x * processScale.x - 0.5f) * resolution.x;
            float newProcessedY = (rawPosition.y * processScale.y - 0.5f) * -resolution.y;
            delta.x = newProcessedX - position.x;
            delta.y = newProcessedY - position.y;
            position.x = newProcessedX;
            position.y = newProcessedY;
        }
    }
}