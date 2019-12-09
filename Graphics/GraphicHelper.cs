using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics
{
    public static class GraphicHelper {

        /// <summary>
        /// Returns the min anchor value for the specified anchor type.
        /// </summary>
        public static Vector2 GetMinAnchor(Anchors anchor)
        {
            switch (anchor)
            {
                case Anchors.TopLeft: return new Vector2(0f, 1f);
                case Anchors.Top: return new Vector2(0.5f, 1f);
                case Anchors.TopRight: return new Vector2(1f, 1f);

                case Anchors.Left: return new Vector2(0f, 0.5f);
                case Anchors.Center: return new Vector2(0.5f, 0.5f);
                case Anchors.Right: return new Vector2(1f, 0.5f);

                case Anchors.BottomLeft: return new Vector2(0f, 0f);
                case Anchors.Bottom: return new Vector2(0.5f, 0f);
                case Anchors.BottomRight: return new Vector2(1f, 0f);

                case Anchors.TopStretch: return new Vector2(0f, 1f);
                case Anchors.MiddleStretch: return new Vector2(0f, 0.5f);
                case Anchors.BottomStretch: return new Vector2(0f, 0f);

                case Anchors.LeftStretch: return new Vector2(0f, 0f);
                case Anchors.CenterStretch: return new Vector2(0.5f, 0f);
                case Anchors.RightStretch: return new Vector2(1f, 0f);

                case Anchors.Fill: return new Vector2(0f, 0f);
            }
            throw new ArgumentException($"Unknown anchor type: {anchor}");
        }

        /// <summary>
        /// Returns the max anchor value for the specified anchor type.
        /// </summary>
        public static Vector2 GetMaxAnchor(Anchors anchor)
        {
            switch (anchor)
            {
                case Anchors.TopLeft: return new Vector2(0f, 1f);
                case Anchors.Top: return new Vector2(0.5f, 1f);
                case Anchors.TopRight: return new Vector2(1f, 1f);

                case Anchors.Left: return new Vector2(0f, 0.5f);
                case Anchors.Center: return new Vector2(0.5f, 0.5f);
                case Anchors.Right: return new Vector2(1f, 0.5f);

                case Anchors.BottomLeft: return new Vector2(0f, 0f);
                case Anchors.Bottom: return new Vector2(0.5f, 0f);
                case Anchors.BottomRight: return new Vector2(1f, 0f);

                case Anchors.TopStretch: return new Vector2(1f, 1f);
                case Anchors.MiddleStretch: return new Vector2(1f, 0.5f);
                case Anchors.BottomStretch: return new Vector2(1f, 0f);

                case Anchors.LeftStretch: return new Vector2(0f, 1f);
                case Anchors.CenterStretch: return new Vector2(0.5f, 1f);
                case Anchors.RightStretch: return new Vector2(1f, 1f);

                case Anchors.Fill: return new Vector2(1f, 1f);
            }
            throw new ArgumentException($"Unknown anchor type: {anchor}");
        }

        /// <summary>
        /// Returns the min and max anchor values for the specified anchor type.
        /// x,y = min
        /// z,w = max
        /// </summary>
        public static Vector4 GetFullAnchor(Anchors anchor)
        {
            Vector2 min = GetMinAnchor(anchor);
            Vector2 max = GetMaxAnchor(anchor);
            return new Vector4(min.x, min.y, max.x, max.y);
        }

        /// <summary>
        /// Returns the pivot value for specified pivot type.
        /// </summary>
        public static Vector2 GetPivot(Pivots pivot)
        {
            switch (pivot)
            {
                case Pivots.TopLeft: return new Vector2(0f, 1f);
                case Pivots.Top: return new Vector2(0.5f, 1f);
                case Pivots.TopRight: return new Vector2(1f, 1f);

                case Pivots.Left: return new Vector2(0f, 0.5f);
                case Pivots.Center: return new Vector2(0.5f, 0.5f);
                case Pivots.Right: return new Vector2(1f, 0.5f);

                case Pivots.BottomLeft: return new Vector2(0f, 0f);
                case Pivots.Bottom: return new Vector2(0.5f, 0f);
                case Pivots.BottomRight: return new Vector2(1f, 0f);
            }
            throw new ArgumentException($"Unknown pivot type: {pivot}");
        }
    }
}