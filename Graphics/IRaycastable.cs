namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can receive a raycast event.
    /// </summary>
    public interface IRaycastable {
    
        /// <summary>
        /// Whether this object should receive raycast events.
        /// </summary>
        bool IsRaycastTarget { get; set; }
    }
}