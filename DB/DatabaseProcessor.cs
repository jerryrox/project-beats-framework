using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class DatabaseProcessor<T> : IDatabaseProcessor<T>
        where T : class, IDatabaseEntity, new()
    {
        private object locker;
        private bool disposed = false;

        private IDatabase<T> database;

        private IDatabaseIndex<T> index;
        private FileInfo indexFile;
        private DirectoryInfo dataDirectory;


        public IDatabaseIndex<T> Index { get { lock (locker) { return index; } } }


        public DatabaseProcessor(IDatabase<T> database)
        {
            this.database = database;

            if (!database.Directory.Exists)
            {
                database.Directory.Create();
                database.Directory.Refresh();
            }
            indexFile = new FileInfo(Path.Combine(database.Directory.FullName, "index.dbi"));
            dataDirectory = new DirectoryInfo(Path.Combine(database.Directory.FullName, "data"));
            if (!dataDirectory.Exists)
            {
                dataDirectory.Create();
                dataDirectory.Refresh();
            }
        }

        public void WhileLocked(Action action)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));
            if(action == null) throw new ArgumentNullException(nameof(action));

            lock (locker)
            {
                action.Invoke();
            }
        }

        public FileInfo[] GetDataFiles()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            return dataDirectory.GetFiles("*.data");
        }

        public void RebuildIndex()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            lock (locker)
            {
                index = new DatabaseIndex<T>(this);
            }
        }

        public void SaveIndex()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            lock (locker)
            {
                File.WriteAllText(indexFile.FullName, JsonConvert.SerializeObject(index.Raw));
                indexFile.Refresh();
            }
        }

        public void LoadIndex()
        {
            lock (locker)
            {
                var rawData = indexFile.Exists ? File.ReadAllText(indexFile.FullName) : "[]";
                var indexList = JsonConvert.DeserializeObject<List<JObject>>(rawData);
                index = new DatabaseIndex<T>(indexList);
            }
        }

        public void WriteData(List<JObject> data, List<JObject> index)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            lock (locker)
            {
                for (int i = 0; i < index.Count; i++)
                {
                    this.index.Set(index[i]);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    File.WriteAllText(GetDataPath(data[i]["Id"].ToString()), data[i].ToString());
                }
            }
        }

        public void RemoveData(List<T> data)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            lock (locker)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    index.Remove(data[i].Id.ToString());
                }
            }
        }

        public JObject LoadRaw(string key, bool requireLock = true)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            Func<JObject> process = () =>
            {
                var path = GetDataPath(key);
                if(!File.Exists(key))
                    throw new FileNotFoundException();
                return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path));
            };

            if (requireLock)
            {
                lock (locker)
                {
                    return process();
                }
            }
            return process();
        }

        public T LoadData(string key, bool requireLock = true)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            return ConvertToData(LoadRaw(key, requireLock));
        }

        public T ConvertToData(JObject raw)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseProcessor<T>));

            return raw.ToObject<T>();
        }

        public void Dispose()
        {
            disposed = true;
            locker = null;
            database = null;
            index = null;
            indexFile = null;
            dataDirectory = null;
        }

        /// <summary>
        /// Resolves the specified data key into a data file path.
        /// </summary>
        private string GetDataPath(string key)
        {
            return Path.Combine(database.Directory.FullName, $"{key}.data");
        }
    }
}