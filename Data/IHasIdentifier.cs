using System;

namespace PBFramework.Data
{
    /// <summary>
    /// Indicates that the object has a unique GUID.
    /// </summary>
    public interface IHasIdentifier
    {
        /// <summary>
        /// Unique identifier of this object.
        /// </summary>
        Guid Id { get; set; }
    }
}