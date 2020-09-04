using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PBFramework.IO;
using PBFramework.IO.Compressed;
using PBFramework.DB;
using PBFramework.Data;
using PBFramework.Storages;
using PBFramework.Threading;
using PBFramework.Debugging;
using PBFramework.Exceptions;

namespace PBFramework.Stores
{
    /// <summary>
    /// Store backed with DirectoryStorage and DatabaseStorage.
    /// </summary>
    public abstract class DirectoryBackedStore<T> : IDirectoryBackedStore<T>
        where T : class, IDirectoryIndex, new()
    {
        public event Action<T> OnNewData;
        public event Action<T> OnRemoveData;

        private IDatabase<T> database;
        private IDirectoryStorage storage;
        private IDirectoryStorage tempStorage;


        public int Count => database.Query().GetResult().Count;

        protected IDatabase<T> Database => database;

        protected IDirectoryStorage Storage => storage;


        protected DirectoryBackedStore()
        {
            tempStorage = new TempDirectoryStorage(GetType().Name);
        }

        public virtual Task Reload(TaskListener listener = null)
        {
            return Task.Run(() =>
            {
                InitModules(true);
                LoadOrphanedData(listener);
            });
        }

        public async Task<T> Import(FileInfo archive, bool deleteOnImport = true, TaskListener<T> listener = null)
        {
            if (archive == null) throw new ArgumentNullException(nameof(archive));
            if (!archive.Exists) throw new FileNotFoundException($"File at ({archive.FullName}) does not exist!");

            // Retrieve the compressed file representation of the archive.
            var compressed = CompressedHelper.GetCompressed(archive);
            if (compressed == null) throw new NotImportableException(archive, GetType());

            // Start extraction of archive.
            var extractedDir = await compressed.Uncompress(GetTempExtractDir(archive), listener?.CreateSubListener<DirectoryInfo>());
            if (!extractedDir.Exists) throw new NotImportableException(archive, GetType());

            // Parse the data at temporary extraction destination.
            var data = ParseData(extractedDir);
            // Failed to parse.
            if (data == null)
            {
                listener?.SetFinished();
                return default(T);
            }

            // Calculate hash code.
            data.CalculateHash();

            // Check whether this data already exists using hash check.
            bool isNewData = false;
            if (ContainsHash(data.HashCode, out T existingData))
            {
                // Replace existing data.
                PostProcessData(data, existingData.Id);
            }
            else
            {
                // Allocate a new Id.
                PostProcessData(data, Guid.NewGuid());
                isNewData = true;
            }

            // Move the extracted data under management of the storage.
            storage.Move(data.Id.ToString(), extractedDir);
            // Replace or add the data to database.
            database.Edit().Write(data).Commit();

            // Delete archive
            if (deleteOnImport)
                archive.Delete();

            // Report finished.
            listener?.SetValue(data);
            listener?.SetFinished();
            if(isNewData)
                OnNewData?.Invoke(data);
            return data;
        }

        public void Delete(T data)
        {
            database.Edit().Remove(data).Commit();
            storage.Delete(data.Id.ToString());
            OnRemoveData?.Invoke(data);
        }

        public void DeleteAll()
        {
            database.Wipe();
            storage.DeleteAll();
        }

        public IEnumerable<T> GetAll()
        {
            using (var edit = database.Edit())
            {
                using (var result = database.Query().Preload().GetResult())
                {
                    foreach (var r in result)
                    {
                        PostProcessData(r);
                        if (!r.Directory.Exists)
                            edit.Remove(r);
                        else
                            yield return r;
                    }
                }
                edit.Commit();
            }
        }

        public IEnumerable<T> Get(Func<IDatabaseQuery<T>, IDatabaseQuery<T>> query)
        {
            if(query == null) throw new ArgumentNullException(nameof(query));

            using (var edit = database.Edit())
            {
                using (var result = query.Invoke(database.Query()).GetResult())
                {
                    foreach (var r in result)
                    {
                        PostProcessData(r);
                        if(!r.Directory.Exists)
                            edit.Remove(r);
                        else
                            yield return r;
                    }
                }
                edit.Commit();
            }
        }

        public T LoadData(T rawData)
        {
            // Find the directory with the data's id.
            var dir = storage.Get(rawData.Id.ToString());
            if (dir == null)
            {
                Logger.LogError($"DirectoryBackedStore.LoadStoredData - Failed to load data at directory: {dir.FullName}");
                return null;
            }

            // Parse and process data
            rawData = ParseData(dir, rawData);
            PostProcessData(rawData);
            return rawData;
        }

        /// <summary>
        /// Returns a new instance of the database for the store.
        /// </summary>
        protected abstract IDatabase<T> CreateDatabase();

        /// <summary>
        /// Returns a new instance of the directory storage for the store.
        /// </summary>
        protected abstract IDirectoryStorage CreateStorage();

        /// <summary>
        /// Tries parsing data T out of specified directory.
        /// data may be null if new or orphaned data.
        /// </summary>
        protected abstract T ParseData(DirectoryInfo directory, T data = null);

        /// <summary>
        /// Post-processes the data after parsing from directory or loading from database.
        /// </summary>
        protected virtual void PostProcessData(T data, Guid? id = null)
        {
            if(data == null)
                return;

            if(id.HasValue)
                data.Id = id.Value;
            data.Directory = new DirectoryInfo(Path.Combine(storage.Container.FullName, data.Id.ToString()));
        }

        /// <summary>
        /// Prepares the necessary modules for the store to function.
        /// If shouldReload, the modules will be loaded again even if they are already initialized.
        /// </summary>
        protected void InitModules(bool shouldReload)
        {
            if (!shouldReload && database != null && storage != null)
                return;

            if (database != null)
                database.Dispose();

            // Load database and storage.
            database = CreateDatabase();
            storage = CreateStorage();
            if (database == null) throw new NullReferenceException("DirectoryBackedStore.LoadModules - Database is null!");
            if (storage == null) throw new NullReferenceException("DirectoryBackedStore.LoadModules - Storage is null!");

            // Initialize the database.
            database.Initialize();
        }

        /// <summary>
        /// Returns the temporary extraction directory for specified archive.
        /// </summary>
        private DirectoryInfo GetTempExtractDir(FileInfo file)
        {
            return new DirectoryInfo(Path.Combine(tempStorage.Container.FullName, file.GetNameWithoutExtension()));
        }

        /// <summary>
        /// Tries loading all orphaned data which exist in the directory storage but somehow not indexed in the database.
        /// </summary>
        private void LoadOrphanedData(TaskListener listener)
        {
            var directoryList = new List<DirectoryInfo>(storage.GetAll());
            for (int i = 0; i < directoryList.Count; i++)
            {
                var dir = directoryList[i];

                // Report on the progress.
                listener?.SetProgress((float)i / directoryList.Count);

                // Find an entry in the database with matching directory name against index Id.
                using (var result = database.Query().Where(inx => inx["Id"].ToString().Equals(dir.Name)).GetResult())
                {
                    // If already exists, just continue.
                    if (result.Count > 0) continue;

                    // Else, we must adopt this result.
                    var data = ParseData(dir);
                    
                    // If this is not a valid data, delete the directory.
                    if (data == null)
                    {
                        storage.Delete(dir.Name);
                        continue;
                    }

                    // Calculate hashcode
                    data.CalculateHash();

                    // Allocate a new id for data.
                    PostProcessData(data, Guid.NewGuid());

                    // Move the old directory to new directory name.
                    dir.MoveTo(Path.Combine(dir.Parent.FullName, data.Id.ToString()));

                    // Register this data as a new entry.
                    database.Edit().Write(data).Commit();
                    OnNewData?.Invoke(data);
                    Logger.Log($"DirectoryBackedStore.LoadOrphanedData - Successfully adopted orphaned data at: {dir.FullName}");
                }
            }
            listener?.SetFinished();
        }

        /// <summary>
        /// Returns whether the indexed data in database contains the specified hash.
        /// If exists, it will also return the data as out param.
        /// </summary>
        private bool ContainsHash(int hashCode, out T result)
        {
            using (var r = database.Query().Where(inx => inx["HashCode"].Value<int>() == hashCode).GetResult())
            {
                if (r.Count > 0 && r.MoveNext())
                {
                    result = r.Current;
                    return true;
                }
                result = null;
                return false;
            }
        }
    }
}