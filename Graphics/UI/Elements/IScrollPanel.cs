namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a view which can be used to scroll inner elements.
    /// </summary>
    public interface IScrollPanel : IPanel {

        /// <summary>
        /// Movement direction of the panel.
        /// </summary>
        Directions Movement { get; set; }

        /// <summary>
        /// Dragging effect.
        /// </summary>
        DragTypes Drag { get; set; }

        
    }
}