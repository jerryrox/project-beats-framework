using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Storages
{
    public class PrefStorage : IPrefStorage {

        public event Action<string> OnAdded;

        public event Action<string> OnRemoved;

        private JObject json;
        private string id;


        public int Count => json.Count;


        public PrefStorage(string id)
        {
            this.id = id;
            try
            {
                json = JsonConvert.DeserializeObject<JObject>(PlayerPrefs.GetString(id, "{}"));
            }
            catch (Exception e)
            {
                Logger.LogError($"PrefStorage - Failed to parse prefs of id ({id}): {e.Message}");
                json = new JObject();
            }
        }

        public bool Exists(string name) => json.ContainsKey(name);

        public string GetString(string key, string defaultValue = null)
        {
            if(!json.ContainsKey(key))
                return defaultValue;
            return json[key].ToString();
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if(!json.ContainsKey(key))
                return defaultValue;
            return json[key].Value<int>();
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            if(!json.ContainsKey(key))
                return defaultValue;
            return json[key].Value<float>();
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if(!json.ContainsKey(key))
                return defaultValue;
            return json[key].Value<bool>();
        }

        public T GetObject<T>(string key)
        {
            if(!json.ContainsKey(key))
                return default(T);
            return json[key].ToObject<T>();
        }

        public T GetEnum<T>(string key, T defaultValue = default)
            where T : struct
        {
            if(!json.ContainsKey(key))
                return defaultValue;
            if(Enum.TryParse<T>(json[key].ToString(), true, out T result))
                return result;
            return defaultValue;
        }

        public void SetString(string key, string value)
        {
            json[key] = value;
            OnAdded?.Invoke(key);
        }

        public void SetInt(string key, int value)
        {
            json[key] = value;
            OnAdded?.Invoke(key);
        }

        public void SetFloat(string key, float value)
        {
            json[key] = value;
            OnAdded?.Invoke(key);
        }

        public void SetBool(string key, bool value)
        {
            json[key] = value;
            OnAdded?.Invoke(key);
        }

        public void SetObject<T>(string key, T value)
        {
            json[key] = (JObject)JToken.FromObject(value);
            OnAdded?.Invoke(key);
        }

        public void SetEnum<T>(string key, T value) where T : struct
        {
            json[key] = value.ToString();
            OnAdded?.Invoke(key);
        }

        public void Delete(string name)
        {
            json.Remove(name);
            OnRemoved?.Invoke(name);
        }

        public void DeleteAll()
        {
            // Invoke removed for all entries
            foreach(var pair in json)
                OnRemoved?.Invoke(pair.Key);

            json = new JObject();
        }

        public void Save()
        {
            PlayerPrefs.SetString(id, json.ToString());
            PlayerPrefs.Save();
        }
    }
}