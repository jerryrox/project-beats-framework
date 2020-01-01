using UnityEngine;

namespace PBFramework.Animations.Interpolations
{
    public class Vector2Interpolator : IInterpolator<Vector2> {

        public static Vector2Interpolator Instance { get; } = new Vector2Interpolator();


        public Vector2 Interpolate(Vector2 from, Vector2 to, float t)
        {
            to.x = (to.x - from.x) * t + from.x;
            to.y = (to.y - from.y) * t + from.y;
            return to;
        }
    }
}