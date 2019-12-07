namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a depth to adjust rendering order.
    /// </summary>
    public interface IHasDepth {
    
        /// <summary>
        /// Depth value of the object which adjusts rendering order.
        /// </summary>
        int Depth { get; set; }
    }
}