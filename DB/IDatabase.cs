using System;
using System.IO;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Represents the database as viewed by a general user (developer).
    /// </summary>
    public interface IDatabase<T> : IDisposable
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Returns whether the database instance is currently alive (can be used).
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Returns the directory of the database.
        /// </summary>
        DirectoryInfo Directory { get; }


        /// <summary>
        /// Initializes the database.
        /// Returns whether the initialization was a success.
        /// </summary>
        bool Initialize();

        /// <summary>
        /// Returns the editor instance for mutating the database.
        /// </summary>
        IDatabaseEditor<T> Edit();

        /// <summary>
        /// Returns a new object for querying data from the database.
        /// </summary>
        IDatabaseQuery<T> Query();
    }
}