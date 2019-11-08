using System;
using System.Collections.Generic;

namespace PBFramework.Data.Queries
{
    /// <summary>
    /// Delegate for handling query process using specified token.
    /// </summary>
    public delegate IEnumerable<T> QueryHandler<T>(IEnumerable<T> current, string token);

    /// <summary>
    /// Indicates that this object is a query handler for a specific data type T.
    /// </summary>
    public interface IQueryer<T> where T : class, IQueryableData {

        /// <summary>
        /// Returns all objects T whose predicate satisfies the specified query string.
        /// </summary>
        IEnumerable<T> Query(IEnumerable<T> objects, string queryString = "");

		/// <summary>
		/// Adds a special routine to process for tokens which start with specified delimiter.
		/// </summary>
        void SetSpecialHandler(string delimiter, QueryHandler<T> handler);
    }
}