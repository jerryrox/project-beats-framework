namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be modified using transfomation.
    /// </summary>
    public interface IHasTransform : IHasRect, IHasRotation, IHasScale {

        /// <summary>
        /// Pivot point of the rect.
        /// </summary>
        Pivots Pivot { get; set; }

        /// <summary>
        /// Anchoring of the transform.
        /// </summary>
        Anchors Anchor { get; set; }
    }
}