using System;

namespace PBFramework.Storages
{
    /// <summary>
    /// Provides the minimal, shared interface across different storage implementations.
    /// </summary>
    public interface IStorage {

        /// <summary>
        /// Event called on addition of a new data in the storage.
        /// </summary>
        event Action<string> OnAdded;

        /// <summary>
        /// Event called on removal of a data in the storage.
        /// </summary>
        event Action<string> OnRemoved;


        /// <summary>
        /// Returns the number of data stored.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Returns whether there is a directory of specified name.
        /// </summary>
        bool Exists(string name);

        /// <summary>
        /// Deletes the directory associated with specified name.
        /// </summary>
        void Delete(string name);

        /// <summary>
        /// Deletes all directories within the storage.
        /// </summary>
        void DeleteAll();
    }
}