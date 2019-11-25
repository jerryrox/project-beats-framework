using System.IO;
using System.Collections.Generic;

namespace PBFramework.Storages
{
    /// <summary>
    /// Interface for storage types that store many subdirectories as data.
    /// </summary>
    public interface IDirectoryStorage {

        /// <summary>
        /// Attempts to restore all backup directories which are left over by the process due to an error.
        /// Returns a list of all names of the directories which have been rescued.
        /// </summary>
        List<string> RestoreBackup();

        /// <summary>
        /// Returns whether there is a directory of specified name.
        /// </summary>
        bool Exists(string name);

        /// <summary>
        /// Returns the information of the direcotry stored at specified name.
        /// </summary>
        DirectoryInfo Get(string name);

        /// <summary>
        /// Moves the source directory under the managed directory of the storage using specified name.
        /// </summary>
        void Move(string name, DirectoryInfo source);

        /// <summary>
        /// Copies the source directory under the managed directory of the storage using specified name.
        /// </summary>
        void Copy(string name, DirectoryInfo source, bool overwrite);
    }
}