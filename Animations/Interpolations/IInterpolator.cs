namespace PBFramework.Animations.Interpolations
{
    public interface IInterpolator<T> {

        /// <summary>
        /// Returns the interpolated value between from and to with the interpolant t.
        /// </summary>
        T Interpolate(T from, T to, float t);
    }
}