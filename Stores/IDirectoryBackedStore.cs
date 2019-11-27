using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBFramework.DB;
using PBFramework.Threading;

namespace PBFramework.Stores
{
    public interface IDirectoryBackedStore<T> 
        where T : class, IDirectoryIndex, new()
    {
        /// <summary>
        /// Event called when a new data has been added to the store.
        /// </summary>
        event Action<T> OnNewData;

        /// <summary>
        /// Event called when an existing data has been removed from the store.
        /// Will not be called when delete using DeleteAll().
        /// </summary>
        event Action<T> OnRemoveData;

        /// <summary>
        /// Returns the number of data currently indexed in the store.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Reloads the store from the file system.
        /// </summary>
        Task Reload(IEventProgress progress = null);

        /// <summary>
        /// Tries importing the specified zip archive as a a new data.
        /// Returns the imported data if successful.
        /// Otherwise, a null value is returned.
        /// </summary>
        Task<T> Import(FileInfo archive, bool deleteOnImport = true, IReturnableProgress<T> progress = null);

        /// <summary>
        /// Deletes the directory associated with the specified data.
        /// </summary>
        void Delete(T data);

        /// <summary>
        /// Deletes all directories in the storage.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Returns all data currently loaded into memory from store.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Returns all data matching the specified query.
        /// </summary>
        IEnumerable<T> Get(Func<IDatabaseQuery<T>, IDatabaseQuery<T>> query);
    }
}