using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Storages
{
    public class FileStorage : IFileStorage {

        protected readonly DirectoryInfo directory;


        public FileStorage(DirectoryInfo directory)
        {
            this.directory = directory;
        }

        public bool Exists(string name) => File.Exists(GetFullPath(name));

        public FileInfo GetFile(string name) => new FileInfo(GetFullPath(name));

        public string GetText(string name)
        {
            string path = GetFullPath(name);
            if(File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }

        public byte[] GetData(string name)
        {
            string path = GetFullPath(name);
            if(File.Exists(path))
                return File.ReadAllBytes(path);
            return null;
        }

        public virtual void Write(string name, string text) => File.WriteAllText(GetFullPath(name), text);

        public virtual void Write(string name, byte[] data) => File.WriteAllBytes(GetFullPath(name), data);

        protected virtual string GetFullPath(string name) => Path.Combine(directory.FullName, name);
    }
}