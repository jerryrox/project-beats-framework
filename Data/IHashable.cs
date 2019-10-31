using System.Collections;

namespace PBFramework.Data
{
    /// <summary>
    /// Indicates the availability of a hashing feature on an object.
    /// </summary>
    public interface IHashable
    {
        /// <summary>
        /// Hash value of the object.
        /// </summary>
        int HashCode { get; set; }


        /// <summary>
        /// Returns all parameters targetted for hashing.
        /// </summary>
        IEnumerable GetHashParams();
    }
}