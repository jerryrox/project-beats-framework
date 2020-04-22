using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Extension class which provides extra functions in a graphical context.
    /// </summary>
    public static class GraphicExtensions {

        public static void SetColorR(this Graphic context, float r)
        {
            var color = context.color;
            color.r = r;
            context.color = color;
        }

        public static void SetColorG(this Graphic context, float g)
        {
            var color = context.color;
            color.g = g;
            context.color = color;
        }

        public static void SetColorB(this Graphic context, float b)
        {
            var color = context.color;
            color.b = b;
            context.color = color;
        }

        public static void SetAlpha(this Graphic context, float a)
        {
            var color = context.color;
            color.a = a;
            context.color = color;
        }

        /// <summary>
        /// Sets this color's RGB values to the specified tint without touching alpha.
        /// </summary>
        public static void SetTint(this Color context, Color tint)
        {
            context.r = tint.r;
            context.g = tint.g;
            context.b = tint.b;
        }

        /// <summary>
        /// Returns a new vector translated by the specified delta.
        /// </summary>
        public static Vector2 Translated(this Vector2 context, float x, float y)
        {
            return new Vector2(context.x + x, context.y + y);
        }
    }
}