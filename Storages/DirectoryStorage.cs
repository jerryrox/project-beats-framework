using System;
using System.IO;
using System.Collections.Generic;
using PBFramework.Utils;
using PBFramework.Debugging;

namespace PBFramework.Storages
{
    public class DirectoryStorage : IDirectoryStorage {

        protected readonly DirectoryInfo directory;


        public DirectoryInfo Container => directory;


        public DirectoryStorage(DirectoryInfo directory)
        {
            this.directory = directory;
        }

        public List<string> RestoreBackup()
        {
            List<string> names = new List<string>();
            var backups = FindBackupDirectories();
            for (int i = 0; i < backups.Length; i++)
            {
                var backupName = backups[i].Name;
                var originalName = FromBackupName(backupName);
                try
                {
                    var originalDir = new DirectoryInfo(GetFullPath(originalName));
                    // If somehow the original directory exists, remove the backup path.
                    if (originalDir.Exists)
                    {
                        Logger.Log($"DirectoryStorage.RestoreBackup - Original directory somehow exists at {originalDir.FullName}.");
                        backups[i].Delete(true);
                        continue;
                    }
                    // Move backup directory to original directory.
                    backups[i].MoveTo(originalDir.FullName);
                    names.Add(originalName);
                }
                catch (Exception e)
                {
                    Logger.LogError($"DirectoryStorage.RestoreBackup - Failed to restore backup at {backups[i].FullName}.\nReason: {e.Message}");
                }
            }
            return names;
        }

        public bool Exists(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("name mustn't be null or empty!");

            return Directory.Exists(GetFullPath(name));
        }

        public DirectoryInfo Get(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("name mustn't be null or empty!");

            return new DirectoryInfo(GetFullPath(name));
        }

        public IEnumerable<DirectoryInfo> GetAll()
        {
            foreach (var dir in directory.GetDirectories())
            {
                if(!IsBackup(dir.Name))
                    yield return dir;
            }
        }

        public void Move(string name, DirectoryInfo source)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("name mustn't be null or empty!");
            if (!source.Exists) throw new DirectoryNotFoundException();

            // Create a copy of the info, so the argument source will not be changed.
            source = new DirectoryInfo(source.FullName);

            SafeWriteDirectory(name, (targetPath) =>
            {
                source.MoveTo(targetPath);
            });
        }

        public void Copy(string name, DirectoryInfo source)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("name mustn't be null or empty!");
            if(!source.Exists) throw new DirectoryNotFoundException();

            SafeWriteDirectory(name, (targetPath) =>
            {
                source.Copy(new DirectoryInfo(targetPath), true);
            });
        }

        public void Delete(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("name mustn't be null or empty!");

            var path = GetFullPath(name);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public void DeleteAll()
        {
            // Delete the managing directory itself
            directory.Delete(true);
            directory.Refresh();

            // Re-create the directory to start fresh.
            directory.Create();
            directory.Refresh();
        }

        /// <summary>
        /// Performs a safe writing process.
        /// </summary>
        private void SafeWriteDirectory(string name, Action<string> action)
        {
            string targetPath = GetFullPath(name);
            string backupPath = GetBackupPath(name);
            try
            {
                if (Directory.Exists(targetPath))
                {
                    if(Directory.Exists(backupPath))
                        Directory.Delete(backupPath, true);
                    Directory.Move(targetPath, backupPath);
                }
                action.Invoke(targetPath);

                if(Directory.Exists(backupPath))
                    Directory.Delete(backupPath, true);
            }
            catch (Exception e)
            {
                if (Directory.Exists(backupPath))
                {
                    if(Directory.Exists(targetPath))
                        Directory.Delete(targetPath, true);

                    Directory.Move(backupPath, targetPath);
                }
                throw e;
            }
        }

        /// <summary>
        /// Returns the full path of the specified directory name.
        /// </summary>
        private string GetFullPath(string name)
        {
            return Path.Combine(directory.FullName, name);
        }

        /// <summary>
        /// Returns the full backup path of the specified directory name.
        /// </summary>
        private string GetBackupPath(string name)
        {
            return Path.Combine(directory.FullName, $"{name}_backup");
        }

        /// <summary>
        /// Returns the actual data name which the specified backup name was built from.
        /// </summary>
        private string FromBackupName(string backupName)
        {
            return backupName.Replace("_backup", "");
        }

        /// <summary>
        /// Finds and returns all backup directories.
        /// </summary>
        private DirectoryInfo[] FindBackupDirectories()
        {
            return directory.GetDirectories("*_backup");
        }

        /// <summary>
        /// Returns whether specified directory name is a backup directory.
        /// </summary>
        private bool IsBackup(string name)
        {
            return name.EndsWith("_backup", StringComparison.OrdinalIgnoreCase);
        }
    }
}