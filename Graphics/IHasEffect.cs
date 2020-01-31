using PBFramework.Graphics.Effects;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be attached with graphic effects.
    /// </summary>
    public interface IHasEffect {

        /// <summary>
        /// Adds the specified effect to this object.
        /// Returns a valid effect instance if successful.
        /// </summary>
        T AddEffect<T>(T effect) where T : class, IEffect;

        /// <summary>
        /// Removes the effect of specified type.
        /// </summary>
        void RemoveEffect<T>() where T : class, IEffect;

        /// <summary>
        /// Returns the effect of specified type, if added to this object.
        /// </summary>
        T GetEffect<T>() where T : class, IEffect;
    }
}