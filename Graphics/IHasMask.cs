namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can mask child graphic objects.
    /// </summary>
    public interface IHasMask {
        
        /// <summary>
        /// Whether masking should be enabled.
        /// </summary>
        bool UseMask { get; set; }

        /// <summary>
        /// Whether the masking texture should be displayed along with the masked children.
        /// </summary>
        bool ShowMaskingSprite { get; set; }
    }
}