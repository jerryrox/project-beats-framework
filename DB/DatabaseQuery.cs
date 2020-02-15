using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class DatabaseQuery<T> : IDatabaseQuery<T>
        where T : class, IDatabaseEntity, new()
    {
        private bool disposed = false;
        private bool fullLoaded = false;

        private IDatabaseProcessor<T> processor;

        private List<FilterHandler> filterHandlers = new List<FilterHandler>();

        private delegate List<JObject> FilterHandler(List<JObject> results);


        public DatabaseQuery(IDatabaseProcessor<T> processor)
        {
            this.processor = processor;
        }

        public IDatabaseQuery<T> Preload()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));

            filterHandlers.Add((list) =>
            {
                return LoadFullData(list);
            });
            return this;
        }

        public IDatabaseQuery<T> Where(Func<JObject, bool> predicate)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));

            filterHandlers.Add((list) =>
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if(!predicate.Invoke(list[i]))
                        list.RemoveAt(i);
                }
                return list;
            });
            return this;
        }

        public IDatabaseQuery<T> Sort(Comparison<JObject> sort)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));
            if(sort == null) throw new ArgumentNullException(nameof(sort));

            filterHandlers.Add((list) =>
            {
                list.Sort(sort);
                return list;
            });
            return this;
        }

        public IDatabaseQuery<T> WhereNonIndexed(Func<JObject, bool> predicate)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));

            filterHandlers.Add((list) =>
            {
                list = LoadFullData(list);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if(!predicate.Invoke(list[i]))
                        list.RemoveAt(i);
                }
                return list;
            });
            return this;
        }

        public IDatabaseQuery<T> SortNonIndexed(Comparison<JObject> sort)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));

            filterHandlers.Add((list) =>
            {
                list = LoadFullData(list);
                list.Sort(sort);
                return list;
            });
            return this;
        }

        public IDatabaseQuery<T> Offset(int count)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));
            if(count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            filterHandlers.Add((list) =>
            {
                if(count >= list.Count)
                    list.Clear();
                else
                    list.RemoveRange(0, count);
                return list;
            });
            return this;
        }

        public IDatabaseQuery<T> Size(int count)
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));
            if(count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            filterHandlers.Add((list) =>
            {
                if(count < list.Count)
                    list.RemoveRange(count, list.Count - count);
                return list;
            });
            return this;
        }

        public IDatabaseResult<T> GetResult()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseQuery<T>));

            // Get all latest index
            var index = processor.Index.GetAll();
            // Apply filtering.
            for (int i = 0; i < filterHandlers.Count; i++)
                index = filterHandlers[i].Invoke(index);

            return new DatabaseResult<T>(
                GetLoadingData(index),
                index.Count
            );
        }

        public void Dispose()
        {
            disposed = true;
            processor = null;
            filterHandlers = null;
        }

        /// <summary>
        /// Returns data which loads the full data on-the-go.
        /// </summary>
        private IEnumerator<T> GetLoadingData(List<JObject> source)
        {
            // If fully loaded, simply convert the Json object to type T.
            if (fullLoaded)
            {
                foreach(var s in source)
                    yield return processor.ConvertToData(s);
            }
            // If not fully loaded, load the data and convert to type T.
            else
            {
                foreach (var s in source)
                    yield return processor.LoadData(s["Id"].ToString(), true);
            }
        }

        /// <summary>
        /// Loads full data via the database IO provider.
        /// </summary>
        private List<JObject> LoadFullData(List<JObject> list)
        {
            // If already fully loaded, just return.
            if(fullLoaded)
                return list;

            // Immediately load full data.
            var newList = new List<JObject>(list.Count);
            processor.WhileLocked(() =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    newList.Add(processor.LoadRaw(list[i]["Id"].ToString(), false));
                }
            });

            // Assign full data to current results.
            return newList;
        }
    }
}