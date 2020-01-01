namespace PBFramework.Storages
{
    public interface IPrefStorage : IStorage {

        /// <summary>
        /// Returns the string value mapped to specified key.
        /// </summary>
        string GetString(string key, string defaultValue = null);

        /// <summary>
        /// Returns the int value mapped to specified key.
        /// </summary>
        int GetInt(string key, int defaultValue = 0);

        /// <summary>
        /// Returns the float value mapped to specified key.
        /// </summary>
        float GetFloat(string key, float defaultValue = 0);

        /// <summary>
        /// Returns the bool value mapped to specified key.
        /// </summary>
        bool GetBool(string key, bool defaultValue = false);

        /// <summary>
        /// Returns the object mapped to specified key.
        /// If fail, a null value will be returned.
        /// </summary>
        T GetObject<T>(string key);

        /// <summary>
        /// Returns the enum value mapped to specified key.
        /// </summary>
        T GetEnum<T>(string key, T defaultValue = default) where T : struct;

        /// <summary>
        /// Sets the string value to specified key.
        /// </summary>
        void SetString(string key, string value);

        /// <summary>
        /// Sets the int value to specified key.
        /// </summary>
        void SetInt(string key, int value);

        /// <summary>
        /// Sets the float value to specified key.
        /// </summary>
        void SetFloat(string key, float value);

        /// <summary>
        /// Sets the bool value to specified key.
        /// </summary>
        void SetBool(string key, bool value);

        /// <summary>
        /// Sets the object to specified key.
        /// </summary>
        void SetObject<T>(string key, T value);

        /// <summary>
        /// Sets the enum value to specified key.
        /// </summary>
        void SetEnum<T>(string key, T value) where T : struct;

        /// <summary>
        /// Saves current data to PlayerPrefs.
        /// </summary>
        void Save();
    }
}