using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PBFramework.DB.Entities
{
    /// <summary>
    /// Interface of an entity as viewed by the database.
    /// </summary>
    public interface IDatabaseEntity {

        /// <summary>
        /// The primary identifier of the entity instance.
        /// </summary>
        Guid Id { get; set; }


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