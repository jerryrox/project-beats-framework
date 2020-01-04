using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBFramework.DB.Entities
{
    /// <summary>
    /// Default implementation of IDatabaseEntity.
    /// </summary>
    public abstract class DatabaseEntity : IDatabaseEntity {

        [PrimaryKey]
        [JsonProperty("Id")]
        public Guid Id { get; set; }



        public virtual void InitializeAsNew()
        {
            Id = Guid.NewGuid();
        }

        public JObject Serialize()
        {
            JObject json = (JObject)JToken.FromObject(this);
            return json;
        }

        public JObject SerializeIndex()
        {
            var serializer = new JsonSerializer()
            {
                ContractResolver = EntityIndexContractResolver.Instance
            };
            JObject json = (JObject)JToken.FromObject(this, serializer);
            return json;
        }
    }
}