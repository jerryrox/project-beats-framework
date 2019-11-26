using System.IO;

namespace PBFramework.Storages
{
    /// <summary>
    /// Interface of an anonymous file access storage.
    /// </summary>
    public interface IFileStorage : IStorage {

        /// <summary>
        /// Returns the directory which contains all the file data in this storage.
        /// </summary>
        DirectoryInfo Container { get; }


        /// <summary>
        /// Returns the information of the file with specified name.
        /// </summary>
        FileInfo GetFile(string name);

        /// <summary>
        /// Returns the text content of the file with specified name.
        /// </summary>
        string GetText(string name);

        /// <summary>
        /// Returns the byte data of the file with specified name.
        /// </summary>
        byte[] GetData(string name);

        /// <summary>
        /// Writes the text value to the file of specified name.
        /// </summary>
        void Write(string name, string text);

        /// <summary>
        /// Writes the raw byte data to the file of specified name.
        /// </summary>
        void Write(string name, byte[] data);
    }
}