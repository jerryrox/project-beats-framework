using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class DatabaseIndex<T> : IDatabaseIndex<T>
        where T :class, IDatabaseEntity, new()
    {

        private const int RebuildDataPerTask = 20;

        private Dictionary<string, JObject> index = new Dictionary<string, JObject>();


        public IEnumerable<JObject> Raw => index.Values;


        public DatabaseIndex(IDatabaseProcessor<T> processor)
        {
            RebuildIndex(processor);
        }

        public DatabaseIndex(List<JObject> indexList)
        {
            foreach (var i in indexList)
            {
                index[i["Id"].ToString()] = i;
            }
        }

        public void Set(JObject index) => this.index[index["Id"].ToString()] = index;

        public void Remove(string key) => index.Remove(key);

        public void Remove(JObject index) => this.index.Remove(index["Id"].ToString());

        public List<JObject> GetAll() => index.Values.ToList();

        /// <summary>
        /// Rebuilds index from scratch using specified data files.
        /// </summary>
        private void RebuildIndex(IDatabaseProcessor<T> processor)
        {
            var dataFiles = processor.GetDataFiles();

            // Task allocation variables
            int taskCount = (dataFiles.Length - 1) / RebuildDataPerTask + 1;
            var tasks = new Task[taskCount];

            // Allocate tasks
            for (int i = 0; i < taskCount; i++)
            {
                int startIndex = i * RebuildDataPerTask;
                int loopCount = Math.Min(RebuildDataPerTask, dataFiles.Length - startIndex);
                tasks[i] = Task.Run(() => BuildIndex(processor, dataFiles, startIndex, loopCount));
            }

            // Wait for all tasks to finish.
            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Handles the index building process for specified range.
        /// </summary>
        private void BuildIndex(IDatabaseProcessor<T> processor, FileInfo[] dataFiles, int startIndex, int loopCount)
        {
            var localIndex = new JObject[loopCount];
            for (int i = 0; i < loopCount; i++)
            {
                var file = dataFiles[i + startIndex];
                localIndex[i] = processor.LoadData(Path.GetFileNameWithoutExtension(file.Name), false).SerializeIndex();
            }

            lock (index)
            {
                for (int i = 0; i < localIndex.Length; i++)
                {
                    Set(localIndex[i]);
                }
            }
        }
    }
}