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
        /// The layer of the gameObject.
        /// </summary>
        int Layer { get; set; }


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
        /// Sets the parent object of this object to specified transform.
        /// </summary>
        void SetParent(Transform transform);

        /// <summary>
        /// Sets the parent object of this object to specified object.
        /// </summary>
        void SetParent(IObject obj);

        /// <summary>
        /// Adds the specified component to the object.
        /// </summary>
        T AddComponent<T>() where T : Component;

        /// <summary>
        /// Destroys this object completely from the game.
        /// </summary>
        void Destroy();
    }
}