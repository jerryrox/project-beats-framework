using System.IO;

namespace PBFramework.IO
{
    /// <summary>
    /// Indicates that the object has an associated directory info.
    /// </summary>
    public interface IHasDirectory {
    
        /// <summary>
        /// The directory associated with this object.
        /// </summary>
        DirectoryInfo Directory { get; set; }
    }
}