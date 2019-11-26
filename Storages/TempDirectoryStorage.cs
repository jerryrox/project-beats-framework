using System.IO;
using UnityEngine;

namespace PBFramework.Storages
{
    /// <summary>
    /// Extension of DirectoryStorage for storing temporary files.
    /// </summary>
    public class TempDirectoryStorage : DirectoryStorage {
    
        public TempDirectoryStorage(string name) : base(new DirectoryInfo(Path.Combine(Application.persistentDataPath, $"temp/{name}")))
        {
            // Delete all data on initialization.
            DeleteAll();
        }
    }
}