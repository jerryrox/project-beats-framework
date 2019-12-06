using System.Collections.Generic;
using PBFramework.Graphics.UI.Elements;

namespace PBFramework.Graphics.UI
{
    /// <summary>
    /// Abstraction of a 2D displayable object in the game.
    /// </summary>
    public interface IDisplay : IObject {

        /// <summary>
        /// Returns the parent display object in the hierarchy.
        /// </summary>
        IDisplay Parent { get; set; }

        /// <summary>
        /// Depth which determines the order of rendering of displays.
        /// </summary>
        int Depth { get; set; }


        /// <summary>
        /// Creates a new 2D child and returns it.
        /// </summary>
        new IDisplay CreateChild();

        /// <summary>
        /// Adds the specified display as a child object.
        /// </summary>
        void AddChild(IDisplay display);

        /// <summary>
        /// Adds the specified displays as children objects.
        /// </summary>
        void AddChildren(IEnumerable<IDisplay> displays);

        /// <summary>
        /// Adds the specified element to this display.
        /// </summary>
        T AddElement<T>(T element) where T : IElement;

        /// <summary>
        /// Returns the element attached on this object.
        /// </summary>
        T GetElement<T>() where T : IElement;

        /// <summary>
        /// Returns the first element found in the child objects.
        /// </summary>
        T GetElementInChildren<T>() where T : IElement;

        /// <summary>
        /// Returns all the elements in the children objects.
        /// </summary>
        IEnumerable<T> GetElementsInChildren<T>() where T : IElement;

        /// <summary>
        /// Returns the element in the parent object.
        /// </summary>
        T GetElementInParent<T>() where T : IElement;
    }
}