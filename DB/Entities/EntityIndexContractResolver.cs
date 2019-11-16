using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PBFramework.DB.Entities
{
    public class EntityIndexContractResolver : DefaultContractResolver {

        public static readonly EntityIndexContractResolver Instance = new EntityIndexContractResolver();


        public EntityIndexContractResolver()
        {
            
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            bool isIndex = property.AttributeProvider.GetAttributes(typeof(IndexedAttribute), true).Count > 0;
            property.Ignored = !isIndex;
            return property;
        }
    }
}