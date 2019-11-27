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
            var text = GetText(name);
            if(string.IsNullOrEmpty(text)) return null;
            return JsonConvert.DeserializeObject<JObject>(text);
        }

        public JArray GetArray(string name)
        {
            var text = GetText(name);
            if(string.IsNullOrEmpty(text)) return null;
            return JsonConvert.DeserializeObject<JArray>(text);
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
            WriteTextAsJson(name, text);
        }

        public override void Write(string name, byte[] data)
        {
            string text = System.Text.Encoding.UTF8.GetString(data);
            WriteTextAsJson(name, text);
        }

        private void WriteTextAsJson(string name, string text)
        {
            text = text.Trim();
            if (text.StartsWith("{") && text.EndsWith("}"))
            {
                Write(name, JsonConvert.DeserializeObject<JObject>(text));
                return;
            }
            else if(text.StartsWith("[") && text.EndsWith("]"))
            {
                Write(name, JsonConvert.DeserializeObject<JArray>(text));
                return;
            }
            throw new InvalidCastException("Specified text could not be converted to a JSON object nor an array!");
        }
    }
}