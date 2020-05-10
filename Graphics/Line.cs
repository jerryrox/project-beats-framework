using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Utils;

namespace PBFramework.Graphics
{
    public class Line {

        /// <summary>
        /// The value of a single radian.
        /// </summary>
        private const float Radian = 180f * Mathf.Deg2Rad;

        private Vector2 endPoint;


        /// <summary>
        /// The starting point reference of the line.
        /// </summary>
        public readonly Vector2 StartPoint;

        /// <summary>
        /// Returns the ending point reference of the line.
        /// </summary>
        public Vector2 EndPoint => endPoint;

        /// <summary>
        /// The rotation of the line in radians.
        /// </summary>
        public readonly float Theta;

        /// <summary>
        /// The direction vector of the line.
        /// </summary>
        public readonly Vector2 Direction;

        /// <summary>
        /// The direction of the line in the right side.
        /// </summary>
        public readonly Vector2 Right;

        /// <summary>
        /// Returns the length of the line.
        /// </summary>
        public float Length => Vector2.Distance(endPoint, StartPoint);


        public Line(Vector2 start, Vector3 end)
        {
            this.StartPoint = start;
            this.endPoint = end;
            this.Theta = Mathf.Atan2(endPoint.y - StartPoint.y, endPoint.x - StartPoint.x);
            Theta += Theta < 0f ? Radian * 2f : 0f;
            this.Direction = new Vector2(endPoint.x - StartPoint.x, endPoint.y - StartPoint.y);

            float angle = Theta + Radian * 0.5f;
            this.Right = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        /// <summary>
        /// Returns the shortest difference in angle between this and the other line.
        /// </summary>
        public float GetAngleDiff(Line other)
        {
            if(MathUtils.AlmostEquals(Theta, other.Theta))
                return 0f;
            float otherDeg = other.Theta * Mathf.Rad2Deg;
            float deg = Theta * Mathf.Rad2Deg;
            float diff = (otherDeg - deg + 180) % 360 - 180;
            return (diff < -180f ? diff + 360 : diff) * Mathf.Deg2Rad;
        }

        /// <summary>
        /// Adjusts the end point of the line to match specified length.
        /// </summary>
        public void SetLength(float length)
        {
            endPoint = StartPoint + (Direction * Mathf.Clamp(length, 0.0000001f, length));
        }
    }
}