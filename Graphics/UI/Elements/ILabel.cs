namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a 2D label.
    /// </summary>
    public interface ILabel : IElement, IHasGradient, IHasSize, IHasDepth {
    
        /// <summary>
        /// The text to be displayed on the label.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Whether ellipsis will be applied to the clamped text.
        /// </summary>
        bool UseEllipsis { get; set; }

        /// <summary>
        /// The type of alignment mode the label will align the texts to.
        /// </summary>
        Alignments Alignment { get; set; }

        /// <summary>
        /// The type of wrap mode to be applied to texts.
        /// </summary>
        SizeWrapModes TextWrap { get; set; }
    }
}