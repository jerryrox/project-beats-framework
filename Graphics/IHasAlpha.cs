namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has an alpha value.
    /// </summary>
    public interface IHasAlpha {
    
        /// <summary>
        /// Direct access to alpha value of the object.
        /// </summary>
        float Alpha { get; set; }
    }
}