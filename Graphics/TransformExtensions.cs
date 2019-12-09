using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics
{
    public static class TransformExtensions {

        public static void SetLocalPositionX(this Transform context, float x)
        {
            var pos = context.localPosition;
            pos.x = x;
            context.localPosition = pos;
        }

        public static void SetLocalPositionY(this Transform context, float y)
        {
            var pos = context.localPosition;
            pos.y = y;
            context.localPosition = pos;
        }

        public static void SetLocalPositionZ(this Transform context, float z)
        {
            var pos = context.localPosition;
            pos.z = z;
            context.localPosition = pos;
        }

        public static void SetLocalPositionXY(this Transform context, float x, float y)
        {
            var pos = context.localPosition;
            pos.x = x;
            pos.y = y;
            context.localPosition = pos;
        }

        public static void SetLocalPositionXZ(this Transform context, float x, float z)
        {
            var pos = context.localPosition;
            pos.x = x;
            pos.z = z;
            context.localPosition = pos;
        }

        public static void SetLocalPositionYZ(this Transform context, float y, float z)
        {
            var pos = context.localPosition;
            pos.y = y;
            pos.z = z;
            context.localPosition = pos;
        }

        public static void SetLocalPositionXYZ(this Transform context, float x, float y, float z)
        {
            var pos = context.localPosition;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            context.localPosition = pos;
        }

        public static void SetLocalEulerX(this Transform context, float x)
        {
            var rot = context.localEulerAngles;
            rot.x = x;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerY(this Transform context, float y)
        {
            var rot = context.localEulerAngles;
            rot.y = y;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerZ(this Transform context, float z)
        {
            var rot = context.localEulerAngles;
            rot.z = z;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerXY(this Transform context, float x, float y)
        {
            var rot = context.localEulerAngles;
            rot.x = x;
            rot.y = y;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerXZ(this Transform context, float x, float z)
        {
            var rot = context.localEulerAngles;
            rot.x = x;
            rot.z = z;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerYZ(this Transform context, float y, float z)
        {
            var rot = context.localEulerAngles;
            rot.y = y;
            rot.z = z;
            context.localEulerAngles = rot;
        }

        public static void SetLocalEulerXYZ(this Transform context, float x, float y, float z)
        {
            var rot = context.localEulerAngles;
            rot.x = x;
            rot.y = y;
            rot.z = z;
            context.localEulerAngles = rot;
        }

        public static void SetLocalScaleX(this Transform context, float x)
        {
            var scale = context.localScale;
            scale.x = x;
            context.localScale = scale;
        }

        public static void SetLocalScaleY(this Transform context, float y)
        {
            var scale = context.localScale;
            scale.y = y;
            context.localScale = scale;
        }

        public static void SetLocalScaleZ(this Transform context, float z)
        {
            var scale = context.localScale;
            scale.z = z;
            context.localScale = scale;
        }

        public static void SetLocalScaleXY(this Transform context, float x, float y)
        {
            var scale = context.localScale;
            scale.x = x;
            scale.y = y;
            context.localScale = scale;
        }

        public static void SetLocalScaleXZ(this Transform context, float x, float z)
        {
            var scale = context.localScale;
            scale.x = x;
            scale.z = z;
            context.localScale = scale;
        }

        public static void SetLocalScaleYZ(this Transform context, float y, float z)
        {
            var scale = context.localScale;
            scale.y = y;
            scale.z = z;
            context.localScale = scale;
        }

        public static void SetLocalScaleXYZ(this Transform context, float x, float y, float z)
        {
            var scale = context.localScale;
            scale.x = x;
            scale.y = y;
            scale.z = z;
            context.localScale = scale;
        }
    }
}