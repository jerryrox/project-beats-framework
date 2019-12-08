using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a rotation.
    /// </summary>
    public interface IHasRotation {
    
        /// <summary>
        /// Direct access to Z euler angle.
        /// </summary>
        float RotationZ { get; set; }

        /// <summary>
        /// Euler angle of the object.
        /// </summary>
        Vector3 Rotation { get; set; }
    }
}