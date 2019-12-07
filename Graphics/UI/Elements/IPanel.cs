namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of an empty view which contains inner elements.
    /// </summary>
    public interface IPanel : IElement, IHasSize, IHasAlpha, IHasDepth {
    
        /// <summary>
        /// The panel's rendering mode.
        /// </summary>
        PanelModes PanelMode { get; set; }
    }
}