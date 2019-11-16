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
        /// Saves the specified data into database.
        /// </summary>
        void Write(T data);

        /// <summary>
        /// Saves all data within the specified range.
        /// </summary>
        void WriteRange(IEnumerable<T> range);

        /// <summary>
        /// Removes the specified data from the database.
        /// </summary>
        void Remove(T data);

        /// <summary>
        /// Removes all data within the specified range.
        /// </summary>
        void RemoveRange(IEnumerable<T> range);

        /// <summary>
        /// Commits the changes queued in the editor.
        /// This object will be disposed afterwards.
        /// </summary>
        void Commit();
    }
}