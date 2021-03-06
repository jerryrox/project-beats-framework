using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be modified using transfomation.
    /// </summary>
    public interface IHasTransform : IHasSize, IHasPosition, IHasRotation, IHasScale {

        /// <summary>
        /// Returns the raw transformation component under the hood.
        /// </summary>
        RectTransform RawTransform { get; }

        /// <summary>
        /// Pivot point of the rect.
        /// </summary>
        PivotType Pivot { get; set; }

        /// <summary>
        /// Anchoring of the transform.
        /// </summary>
        AnchorType Anchor { get; set; }
    }
}