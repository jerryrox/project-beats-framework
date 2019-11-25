using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Interface of a database's internal data processor.
    /// </summary>
    public interface IDatabaseProcessor<T> : IDisposable
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Returns the data index container instance.
        /// </summary>
        IDatabaseIndex<T> Index { get; }


        /// <summary>
        /// Performs the specified action while using the lock.
        /// </summary>
        void WhileLocked(Action action);

        /// <summary>
        /// Returns all data files in the data directory.
        /// </summary>
        FileInfo[] GetDataFiles();

        /// <summary>
        /// Rebuilds the index in-case the index file is somehow corrupted.
        /// </summary>
        void RebuildIndex();

        /// <summary>
        /// Saves current index state to the disk.
        /// </summary>
        void SaveIndex();

        /// <summary>
        /// Loads index state from the disk.
        /// </summary>
        void LoadIndex();

        /// <summary>
        /// Writes the specified range of data to the disk.
        /// </summary>
        void WriteData(List<JObject> data, List<JObject> index);

        /// <summary>
        /// Removes the specified range of data from the database.
        /// </summary>
        void RemoveData(List<T> data);

        /// <summary>
        /// Removes all data stored in the database.
        /// </summary>
        void Wipe();

        /// <summary>
        /// Loads the data associated with specified key as json object.
        /// </summary>
        JObject LoadRaw(string key, bool requireLock = true);

        /// <summary>
        /// Loads the data associated with specified key.
        /// </summary>
        T LoadData(string key, bool requireLock = true);

        /// <summary>
        /// Converts the specified raw json object into entity T.
        /// </summary>
        T ConvertToData(JObject raw);
    }
}