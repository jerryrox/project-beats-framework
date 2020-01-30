using PBFramework.Graphics.Effects;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be attached with graphic effects.
    /// </summary>
    public interface IHasEffect {

        /// <summary>
        /// Returns the current effect set to this object.
        /// </summary>
        IEffect Effect { get; set; }
    }
}