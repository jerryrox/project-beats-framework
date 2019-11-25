using Newtonsoft.Json.Linq;

namespace PBFramework.Storages
{
    /// <summary>
    /// Interface of a Json file access storage.
    /// </summary>
    public interface IJsonStorage : IFileStorage {
        
        /// <summary>
        /// Returns the Json object of the file with specified name.
        /// </summary>
        JObject GetObject(string name);

        /// <summary>
        /// Returns the Json array of the file with specified name.
        /// </summary>
        JArray GetArray(string name);

        /// <summary>
        /// Writes the json object value to the file of specified name.
        /// </summary>
        void Write(string name, JObject json);

        /// <summary>
        /// Writes the json array value to the file of specified name.
        /// </summary>
        void Write(string name, JArray array);
    }
}