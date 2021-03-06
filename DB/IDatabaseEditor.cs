using System;
using System.Collections.Generic;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Interface of a database editor which manages mutation of data.
    /// </summary>
    public interface IDatabaseEditor<T> : IDisposable
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Returns the total number of data write operation registered.
        /// </summary>
        int WriteCount { get; }

        /// <summary>
        /// Returns the total number of data remove operation registered.
        /// </summary>
        int RemoveCount { get; }


        /// <summary>
        /// Saves the specified data into database.
        /// </summary>
        IDatabaseEditor<T> Write(T data);

        /// <summary>
        /// Saves all data within the specified range.
        /// </summary>
        IDatabaseEditor<T> WriteRange(IEnumerable<T> range);

        /// <summary>
        /// Removes the specified data from the database.
        /// </summary>
        IDatabaseEditor<T> Remove(T data);

        /// <summary>
        /// Removes all data within the specified range.
        /// </summary>
        IDatabaseEditor<T> RemoveRange(IEnumerable<T> range);

        /// <summary>
        /// Commits the changes queued in the editor.
        /// This object will be disposed afterwards.
        /// </summary>
        void Commit();
    }
}