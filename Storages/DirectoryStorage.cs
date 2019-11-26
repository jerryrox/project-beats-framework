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

        public bool Exists(string name) => Directory.Exists(GetFullPath(name));

        public DirectoryInfo Get(string name) => new DirectoryInfo(GetFullPath(name));

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
            if (!source.Exists) throw new DirectoryNotFoundException();

            string targetPath = GetFullPath(name);
            string backupPath = GetBackupPath(name);
            if(Directory.Exists(targetPath))
                Directory.Move(targetPath, backupPath);
            source.MoveTo(targetPath);
        }

        public void Copy(string name, DirectoryInfo source, bool overwrite)
        {
            if(!source.Exists) throw new DirectoryNotFoundException();

            DirectoryInfo target = new DirectoryInfo(GetFullPath(name));
            DirectoryInfo backup = new DirectoryInfo(GetBackupPath(name));
            try
            {
                if (target.Exists)
                {
                    if(overwrite)
                        IOUtils.CopyDirectory(target, backup);
                    else
                        target.MoveTo(backup.FullName);
                }
                source.Copy(target);
            }
            catch (Exception e)
            {
                target.Refresh();
                backup.Refresh();
                if(target.Exists && backup.Exists)
                    target.Delete(true);
                throw e;
            }
        }

        public void Delete(string name)
        {
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