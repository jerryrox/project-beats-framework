namespace PBFramework.Graphics.Effects
{
    public interface IEffect {
        
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