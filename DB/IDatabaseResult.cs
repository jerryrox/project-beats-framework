using System;
using System.Collections.Generic;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Object which returns resulting data evaluated from the query.
    /// </summary>
    public interface IDatabaseResult<T> : IEnumerable<T>, IEnumerator<T>, IDisposable
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Returns the total number of data contained in the result.
        /// </summary>
        int Count { get; }
    }
}