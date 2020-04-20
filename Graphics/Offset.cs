using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// A structure which holds size data optimized for PB Framework's graphic object relationship.
    /// </summary>
    public struct Offset {

        private float left;
        private float top;
        private float right;
        private float bottom;


        public float Left
        {
            get => left;
            set => left = value;
        }

        public float Top
        {
            get => top;
            set => top = value;
        }

        public float Right
        {
            get => right;
            set => right = value;
        }

        public float Bottom
        {
            get => bottom;
            set => bottom = value;
        }

        public float Horizontal
        {
            get => left + right;
            set => left = right = value;
        }

        public float Vertical
        {
            get => bottom + top;
            set => bottom = top = value;
        }

        public Vector2 OffsetMin
        {
            get => new Vector2(left, bottom);
            set
            {
                left = value.x;
                bottom = value.y;
            }
        }

        public Vector2 OffsetMax
        {
            get => new Vector2(-right, -top);
            set
            {
                right = -value.x;
                top = -value.y;
            }
        }


        /// <summary>
        /// Creates an offset with specified values for each side.
        /// </summary>
        public Offset(float left, float top, float right, float bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Creates an offset with specified offsetMin and offsetMax from a RectTransform.
        /// </summary>
        public Offset(Vector2 offsetMin, Vector2 offsetMax)
        {
            this.left = offsetMin.x;
            this.top = -offsetMax.y;
            this.right = -offsetMax.x;
            this.bottom = offsetMin.y;
        }
    }
}