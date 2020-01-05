using UnityEngine.UI;

namespace PBFramework.Graphics
{
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
    }
}