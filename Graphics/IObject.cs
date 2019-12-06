using System.Collections.Generic;
using UnityEngine;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Abstraction of any 2D or 3D object that can exist in the game.
    /// </summary>
    public interface IObject : IComponent {
    
        /// <summary>
        /// Name of the object.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Whether the object should be active in the game.
        /// </summary>
        bool IsActive { get; set; }


        /// <summary>
        /// Creates a new child and returns it.
        /// </summary>
        IObject CreateChild();

        /// <summary>
        /// Adds the specified child as a child object.
        /// </summary>
        void AddChild(IObject child);

        /// <summary>
        /// Adds the specified children as children objects.
        /// </summary>
        void AddChildren(IEnumerable<IObject> children);

        /// <summary>
        /// Performs GetComponent on the gameObject instance.
        /// </summary>
        T GetComponent<T>() where T : Component;

        /// <summary>
        /// Performs GetComponentInChildren on the gameObject instance.
        /// </summary>
        T GetComponentInChildren<T>() where T : Component;

        /// <summary>
        /// Performs GetComponentsInChildren on the gameObject instance.
        /// </summary>
        T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component;

        /// <summary>
        /// Performs GetComponentInParent on the gameObject instance.
        /// </summary>
        T GetComponentInParent<T>() where T : Component;

        /// <summary>
        /// Destroys this object completely from the game.
        /// </summary>
        void Destroy();
    }
}