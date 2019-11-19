using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class DatabaseEditor<T> : IDatabaseEditor<T>
        where T : class, IDatabaseEntity, new()
    {
        private bool disposed = false;

        private IDatabaseProcessor<T> processor;

        private List<JObject> writeIndexList;
        private List<JObject> writeList;
        private List<T> removeList;


        public int WriteCount => writeList == null ? 0 : writeList.Count;

        public int RemoveCount => removeList == null ? 0 : removeList.Count;


        public DatabaseEditor(IDatabaseProcessor<T> processor)
        {
            this.processor = processor;
        }

        public void Write(T data)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseEditor<T>));

            if (writeList == null)
            {
                writeList = new List<JObject>();
                writeIndexList = new List<JObject>();
            }
            writeList.Add(data.Serialize());
            writeIndexList.Add(data.SerializeIndex());
        }

        public void WriteRange(IEnumerable<T> range)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseEditor<T>));

            foreach(var data in range)
                Write(data);
        }

        public void Remove(T data)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseEditor<T>));

            if(removeList == null)
                removeList = new List<T>();
            removeList.Add(data);
        }

        public void RemoveRange(IEnumerable<T> range)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseEditor<T>));

            if(removeList == null)
                removeList = new List<T>();
            removeList.AddEnumerable(range);
        }

        public void Commit()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseEditor<T>));

            if(removeList != null)
                processor.RemoveData(removeList);
            if(writeList != null && writeIndexList != null)
                processor.WriteData(writeList, writeIndexList);
            Dispose();
        }

        public void Dispose()
        {
            disposed = true;
            writeList = null;
            writeIndexList = null;
            removeList = null;
        }
    }
}