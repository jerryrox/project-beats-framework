namespace PBFramework.Animations.Interpolations
{
    public class FloatInterpolator : IInterpolator<float> {

        public static FloatInterpolator Instance { get; } = new FloatInterpolator();


        public float Interpolate(float from, float to, float t)
        {
            return (to - from) * t + from;
        }
    }
}