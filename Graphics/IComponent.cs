using UnityEngine;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Interface which provides a service related to encapsulation of Graphic objects' technical details.
    /// </summary>
    public interface IComponent {
    
        /// <summary>
        /// Whether this component should update every frame.
        /// Default: false.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Returns the GameObject component of this component which this is derived from.
        /// </summary>
        GameObject RawObject { get; }

        /// <summary>
        /// Returns the transformation component of this component.
        /// </summary>
        Transform Transform { get; }
    }
}