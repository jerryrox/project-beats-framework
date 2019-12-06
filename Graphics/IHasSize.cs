namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a size property.
    /// </summary>
    public interface IHasSize {

        /// <summary>
        /// The width of the object, if applicable.
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// The height of the object, if applicable.
        /// </summary>
        float Height { get; set; }

        /// <summary>
        /// The pivot where the object will be anchored to, if applicable.
        /// </summary>
        Pivots Pivot { get; set; }


        /// <summary>
        /// Resets the width and height to their original size, if applicable.
        /// </summary>
        void ResetSize();
    }
}