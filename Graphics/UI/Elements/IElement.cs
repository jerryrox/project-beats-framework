using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a component attached to 2D type of objects (display).
    /// </summary>
    public interface IElement : IComponent {
    
        /// <summary>
        /// Returns the display object which the element is attached to.
        /// </summary>
        IDisplay Display { get; }
    }
}