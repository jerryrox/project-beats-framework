using UnityEngine;

namespace PBFramework.Animations.Interpolations
{
    public class Vector3Interpolator : IInterpolator<Vector3> {

        public static Vector3Interpolator Instance { get; } = new Vector3Interpolator();


        public Vector3 Interpolate(Vector3 from, Vector3 to, float t)
        {
            to.x = (to.x - from.x) * t + from.x;
            to.y = (to.y - from.y) * t + from.y;
            to.z = (to.z - from.z) * t + from.z;
            return to;
        }
    }
}