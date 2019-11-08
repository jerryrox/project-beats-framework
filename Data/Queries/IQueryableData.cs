using System.Collections.Generic;

namespace PBFramework.Data.Queries
{
    /// <summary>
    /// Indicates that the object can be queried by an IQuery instance.
    /// </summary>
    public interface IQueryableData {

        /// <summary>
        /// Returns all queryable data bound to this object.
        /// </summary>
        IEnumerable<string> GetQueryables();
    }
}