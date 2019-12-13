using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics
{
    public static class RectTransformExtensions
    {
        public static void SetSizeDeltaX(this RectTransform context, float x)
        {
            var size = context.sizeDelta;
            size.x = x;
            context.sizeDelta = size;
        }

        public static void SetSizeDeltaY(this RectTransform context, float y)
        {
            var size = context.sizeDelta;
            size.y = y;
            context.sizeDelta = size;
        }

        public static void SetSizeDelta(this RectTransform context, float x, float y)
        {
            var size = context.sizeDelta;
            size.x = x;
            size.y = y;
            context.sizeDelta = size;
        }

        /// <summary>
        /// Sets the min and max anchor values for specified anchor type.
        /// </summary>
        public static void SetAnchor(this RectTransform context, Anchors anchor)
        {
            context.anchorMin = GraphicHelper.GetMinAnchor(anchor);
            context.anchorMax = GraphicHelper.GetMaxAnchor(anchor);
        }

        /// <summary>
        /// Sets the pivot values for specified anchor
        /// </summary>
        public static void SetPivot(this RectTransform context, Pivots pivot)
        {
            context.pivot = GraphicHelper.GetPivot(pivot);
        }
    }
}