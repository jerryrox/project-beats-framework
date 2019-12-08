namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has an adjustable alpha.
    /// </summary>
    public interface IHasAlpha {
    
        /// <summary>
        /// Alpha value of the object.
        /// </summary>
        float Alpha { get; set; }
    }
}