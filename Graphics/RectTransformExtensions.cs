using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics
{
    public static class RectTransformExtensions
    {

        public static void SetAnchoredPositionX(this RectTransform context, float x)
        {
            var pos = context.anchoredPosition3D;
            pos.x = x;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionY(this RectTransform context, float y)
        {
            var pos = context.anchoredPosition3D;
            pos.y = y;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionZ(this RectTransform context, float z)
        {
            var pos = context.anchoredPosition3D;
            pos.z = z;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionXY(this RectTransform context, float x, float y)
        {
            var pos = context.anchoredPosition3D;
            pos.x = x;
            pos.y = y;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionXZ(this RectTransform context, float x, float z)
        {
            var pos = context.anchoredPosition3D;
            pos.x = x;
            pos.z = z;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionYZ(this RectTransform context, float y, float z)
        {
            var pos = context.anchoredPosition3D;
            pos.y = y;
            pos.z = z;
            context.anchoredPosition3D = pos;
        }

        public static void SetAnchoredPositionXYZ(this RectTransform context, float x, float y, float z)
        {
            var pos = context.anchoredPosition3D;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            context.anchoredPosition3D = pos;
        }

        public static void SetOffsetLeft(this RectTransform context, float offset)
        {
            context.offsetMin = new Vector2(offset, context.offsetMin.y);
        }

        public static void SetOffsetRight(this RectTransform context, float offset)
        {
            context.offsetMax = new Vector2(offset, context.offsetMax.y);
        }

        public static void SetOffsetTop(this RectTransform context, float offset)
        {
            context.offsetMax = new Vector2(context.offsetMax.x, offset);
        }

        public static void SetOffsetBottom(this RectTransform context, float offset)
        {
            context.offsetMin = new Vector2(context.offsetMin.x, offset);
        }

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