using System.Collections.Generic;
using UnityEngine;
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
        IDisplay Parent { get; }


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
        /// Sets the parent object of specified display to this object.
        /// </summary>
        void SetParent(IDisplay display);
    }
}