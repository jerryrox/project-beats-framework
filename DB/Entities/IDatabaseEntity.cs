using System;
using PBFramework.Data;
using Newtonsoft.Json.Linq;

namespace PBFramework.DB.Entities
{
    /// <summary>
    /// Interface of an entity as viewed by the database.
    /// </summary>
    public interface IDatabaseEntity : IHasIdentifier {

        /// <summary>
        /// Initializes the entity as a new entry to the database.
        /// </summary>
        void InitializeAsNew();

        /// <summary>
        /// Serializes this entity instance into a JSON object.
        /// </summary>
        JObject Serialize();

        /// <summary>
        /// Serializes the indexed data of this entity instance into a JSON object.
        /// </summary>
        JObject SerializeIndex();
    }
}