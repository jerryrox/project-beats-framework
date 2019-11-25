using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PBFramework.Debugging;

namespace PBFramework.Storages
{
    public class JsonStorage : FileStorage, IJsonStorage {
    
        public JsonStorage(DirectoryInfo directory) : base(directory) {}

        public JObject GetObject(string name)
        {
            return JsonConvert.DeserializeObject<JObject>(GetText(name));
        }

        public JArray GetArray(string name)
        {
            return JsonConvert.DeserializeObject<JArray>(GetText(name));
        }

        public void Write(string name, JObject json)
        {
            File.WriteAllText(GetFullPath(name), json.ToString());
        }

        public void Write(string name, JArray array)
        {
            File.WriteAllText(GetFullPath(name), array.ToString());
        }

        public override void Write(string name, string text)
        {
            try
            {
                Write(name, JsonConvert.DeserializeObject<JObject>(text));
            }
            catch (Exception)
            {
                Logger.LogWarning($"JsonStorage.Write - Failed to write text. (Invalid Json format)");
            }
        }

        public override void Write(string name, byte[] data)
        {
            try
            {
                string text = System.Text.Encoding.UTF8.GetString(data);
                Write(name, JsonConvert.DeserializeObject<JObject>(text));
            }
            catch (Exception)
            {
                Logger.LogWarning($"JsonStorage.Write - Failed to write text. (Invalid Json format)");
            }
        }
    }
}