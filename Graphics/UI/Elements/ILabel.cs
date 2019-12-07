namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a 2D label.
    /// </summary>
    public interface ILabel : IElement, IHasColor, IHasSize, IHasDepth {
    
        /// <summary>
        /// The text to be displayed on the label.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The type of wrap mode to be applied to texts.
        /// </summary>
        SizeWrapModes TextWrap { get; set; }
    }
}