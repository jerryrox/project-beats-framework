namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a 2D sprite.
    /// </summary>
    public interface ISprite : IElement, IHasSize, IHasGradient, IHasDepth {

        /// <summary>
        /// Name of the sprite displayed on the object.
        /// </summary>
        string Spritename { get; set; }
    }
}