using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    /// <summary>
    /// Interface to the index data of the database.
    /// </summary>
    public interface IDatabaseIndex<T>
        where T : class, IDatabaseEntity, new()
    {

        /// <summary>
        /// Returns a read-only list of all raw index data.
        /// </summary>
        IEnumerable<JObject> Raw { get; }


        /// <summary>
        /// Sets the specified index to the raw list.
        /// </summary>
        void Set(JObject index);

        /// <summary>
        /// Removes the index entry of specified key.
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// Removes the specified index from the raw list.
        /// </summary>
        void Remove(JObject index);

        /// <summary>
        /// Returns a new list of all index cached in memory.
        /// </summary>
        List<JObject> GetAll();
    }
}