namespace PBFramework.Graphics.Effects
{
    public interface IEffect {

        /// <summary>
        /// Returns whether this effect requires rendering via a custom material.
        /// </summary>
        bool UsesMaterial { get; }


        /// <summary>
        /// Applies effects to the specified object.
        /// Returns whether the effect has successfully been applied.
        /// </summary>
        bool Apply(IGraphicObject obj);

        /// <summary>
        /// Reverts effects from the specified object.
        /// </summary>
        void Revert(IGraphicObject obj);
    }
}