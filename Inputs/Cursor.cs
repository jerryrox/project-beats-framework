using UnityEngine;

namespace PBFramework.Inputs
{
    public abstract class Cursor : ICursor {

        protected readonly int id;

        /// <summary>
        /// Conversion scale applied while converting raw vector to processed vector.
        /// </summary>
        protected readonly Vector2 processScale;

        protected readonly Vector2 resolution;

        protected Vector2 rawPosition = new Vector2();
        protected Vector2 rawDelta = new Vector2();
        protected Vector2 position = new Vector2();
        protected Vector2 delta = new Vector2();


        public uint Id { get; private set; }

        public bool IsActive { get; protected set; }

        public InputState State { get; protected set; } = InputState.Idle;

        public Vector2 RawPosition => rawPosition;

        public Vector2 RawDelta => rawDelta;

        public Vector2 Position => position;

        public Vector2 Delta => delta;


        protected Cursor(uint id, Vector2 resolution)
        {
            this.id = (int)id;
            this.resolution = resolution;

            processScale = new Vector2(1f / Screen.width, 1f / Screen.height);

            Id = id;
        }

        protected void ProcessPosition(float newX, float newY)
        {
            rawDelta.x = newX - rawPosition.x;
            rawDelta.y = newY - rawPosition.y;
            rawPosition.x = newX;
            rawPosition.y = newY;

            float newProcessedX = (rawPosition.x * processScale.x - 0.5f) * resolution.x;
            float newProcessedY = (rawPosition.y * processScale.y - 0.5f) * resolution.y;
            delta.x = newProcessedX - position.x;
            delta.y = newProcessedY - position.y;
            position.x = newProcessedX;
            position.y = newProcessedY;
        }
    }
}