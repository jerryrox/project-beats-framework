using System.Collections.Generic;

namespace PBFramework.Data.Queries
{
    /// <summary>
    /// Indicates that this object is a query handler for a specific data type T.
    /// The queried source is also retained within the implementation.
    /// </summary>
    public interface IStatefulQueryer<T> : IQueryer<T> where T : class, IQueryableData {

        /// <summary>
        /// Returns enumerable of the retained source.
        /// </summary>
        IEnumerable<T> All { get; }


        /// <summary>
        /// Returns all objects T whose predicate satisfies the specified query string.
        /// </summary>
        IEnumerable<T> Query(string queryString = "");
    }
}