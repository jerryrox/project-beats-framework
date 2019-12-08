using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a scale.
    /// </summary>
    public interface IHasScale {

        /// <summary>
        /// Direct access to X scale of the object.
        /// </summary>
        float ScaleX { get; set; }

        /// <summary>
        /// Direct access to Y scale of the object.
        /// </summary>
        /// <value></value>
        float ScaleY { get; set; }

        /// <summary>
        /// Scale of the object.
        /// </summary>
        Vector3 Scale { get; set; }
    }
}