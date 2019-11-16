using System;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Represents an object which queries data from the database.
    /// </summary>
    public interface IDatabaseQuery<T> : IDisposable
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Loads the full data in advance so all the resulting data will already be
        /// loaded by the time results are fetched by user.
        /// Not recommended to call this at the first.
        /// Not required to call this when a "NonIndexed" query method has been called.
        /// </summary>
        IDatabaseQuery<T> Preload();

        /// <summary>
        /// Selects all data which satisfy the specified predicate with indexed properties only.
        /// </summary>
        IDatabaseQuery<T> Where(Func<JObject, bool> predicate);

        /// <summary>
        /// Sorts the results by specified sorting function with indexed properties only.
        /// </summary>
        IDatabaseQuery<T> Sort(Comparison<JObject> sort);

        /// <summary>
        /// Selects all data which satisfy the specified predicate with non-indexed property support.
        /// </summary>
        IDatabaseQuery<T> WhereNonIndexed(Func<JObject, bool> predicate);

        /// <summary>
        /// Sorts the results by specified sorting function with non-indexed property support.
        /// </summary>
        IDatabaseQuery<T> SortNonIndexed(Comparison<JObject> sort);

        /// <summary>
        /// Trims the beginning of the results by specified number of count.
        /// </summary>
        IDatabaseQuery<T> Offset(int count);

        /// <summary>
        /// Trims the ending of the results to clamp the max result count to specified value.
        /// </summary>
        IDatabaseQuery<T> Size(int count);

        /// <summary>
        /// Returns the result evaluated from the queries made on this object.
        /// </summary>
        IDatabaseResult<T> GetResult();
    }
}