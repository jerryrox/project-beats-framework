using System;
using UnityEngine;

namespace PBFramework.Graphics
{
    public static class GraphicHelper {

        /// <summary>
        /// Returns the min anchor value for the specified anchor type.
        /// </summary>
        public static Vector2 GetMinAnchor(AnchorType anchor)
        {
            switch (anchor)
            {
                case AnchorType.TopLeft: return new Vector2(0f, 1f);
                case AnchorType.Top: return new Vector2(0.5f, 1f);
                case AnchorType.TopRight: return new Vector2(1f, 1f);

                case AnchorType.Left: return new Vector2(0f, 0.5f);
                case AnchorType.Center: return new Vector2(0.5f, 0.5f);
                case AnchorType.Right: return new Vector2(1f, 0.5f);

                case AnchorType.BottomLeft: return new Vector2(0f, 0f);
                case AnchorType.Bottom: return new Vector2(0.5f, 0f);
                case AnchorType.BottomRight: return new Vector2(1f, 0f);

                case AnchorType.TopStretch: return new Vector2(0f, 1f);
                case AnchorType.MiddleStretch: return new Vector2(0f, 0.5f);
                case AnchorType.BottomStretch: return new Vector2(0f, 0f);

                case AnchorType.LeftStretch: return new Vector2(0f, 0f);
                case AnchorType.CenterStretch: return new Vector2(0.5f, 0f);
                case AnchorType.RightStretch: return new Vector2(1f, 0f);

                case AnchorType.Fill: return new Vector2(0f, 0f);
            }
            throw new ArgumentException($"Unknown anchor type: {anchor}");
        }

        /// <summary>
        /// Returns the max anchor value for the specified anchor type.
        /// </summary>
        public static Vector2 GetMaxAnchor(AnchorType anchor)
        {
            switch (anchor)
            {
                case AnchorType.TopLeft: return new Vector2(0f, 1f);
                case AnchorType.Top: return new Vector2(0.5f, 1f);
                case AnchorType.TopRight: return new Vector2(1f, 1f);

                case AnchorType.Left: return new Vector2(0f, 0.5f);
                case AnchorType.Center: return new Vector2(0.5f, 0.5f);
                case AnchorType.Right: return new Vector2(1f, 0.5f);

                case AnchorType.BottomLeft: return new Vector2(0f, 0f);
                case AnchorType.Bottom: return new Vector2(0.5f, 0f);
                case AnchorType.BottomRight: return new Vector2(1f, 0f);

                case AnchorType.TopStretch: return new Vector2(1f, 1f);
                case AnchorType.MiddleStretch: return new Vector2(1f, 0.5f);
                case AnchorType.BottomStretch: return new Vector2(1f, 0f);

                case AnchorType.LeftStretch: return new Vector2(0f, 1f);
                case AnchorType.CenterStretch: return new Vector2(0.5f, 1f);
                case AnchorType.RightStretch: return new Vector2(1f, 1f);

                case AnchorType.Fill: return new Vector2(1f, 1f);
            }
            throw new ArgumentException($"Unknown anchor type: {anchor}");
        }

        /// <summary>
        /// Returns the min and max anchor values for the specified anchor type.
        /// x,y = min
        /// z,w = max
        /// </summary>
        public static Vector4 GetFullAnchor(AnchorType anchor)
        {
            Vector2 min = GetMinAnchor(anchor);
            Vector2 max = GetMaxAnchor(anchor);
            return new Vector4(min.x, min.y, max.x, max.y);
        }

        /// <summary>
        /// Returns the pivot value for specified pivot type.
        /// </summary>
        public static Vector2 GetPivot(PivotType pivot)
        {
            switch (pivot)
            {
                case PivotType.TopLeft: return new Vector2(0f, 1f);
                case PivotType.Top: return new Vector2(0.5f, 1f);
                case PivotType.TopRight: return new Vector2(1f, 1f);

                case PivotType.Left: return new Vector2(0f, 0.5f);
                case PivotType.Center: return new Vector2(0.5f, 0.5f);
                case PivotType.Right: return new Vector2(1f, 0.5f);

                case PivotType.BottomLeft: return new Vector2(0f, 0f);
                case PivotType.Bottom: return new Vector2(0.5f, 0f);
                case PivotType.BottomRight: return new Vector2(1f, 0f);
            }
            throw new ArgumentException($"Unknown pivot type: {pivot}");
        }
    }
}