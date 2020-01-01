using UnityEngine;

namespace PBFramework.Animations.Interpolations
{
    public class ColorInterpolator : IInterpolator<Color> {

        public static ColorInterpolator Instance { get; } = new ColorInterpolator();


        public Color Interpolate(Color from, Color to, float t)
        {
            to.r = (to.r - from.r) * t + from.r;
            to.g = (to.g - from.g) * t + from.g;
            to.b = (to.b - from.b) * t + from.b;
            to.a = (to.a - from.a) * t + from.a;
            return to;
        }
    }
}