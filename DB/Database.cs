using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class Database<T> : IDatabase<T>
        where T : class, IDatabaseEntity, new()
    {
        private bool isInitialized = false;
        private bool disposed = false;

        private IDatabaseProcessor<T> processor;


        public bool IsAlive => isInitialized && !disposed;

        public DirectoryInfo Directory { get; private set; }


        /// <summary>
        /// Sets database at default location using specified name.
        /// </summary>
        public Database(string name) : this(new DirectoryInfo(Path.Combine(Application.persistentDataPath, name))) {}

        /// <summary>
        /// Sets database at specified location.
        /// </summary>
        public Database(DirectoryInfo directory)
        {
            Directory = directory;
        }

        public bool Initialize()
        {
            if(disposed) throw new ObjectDisposedException(nameof(Database<T>));
            if(isInitialized) return false;

            processor = new DatabaseProcessor<T>(this);
            processor.LoadIndex();
            isInitialized = true;
            return isInitialized;
        }

        public IDatabaseEditor<T> Edit()
        {
            if(disposed) throw new ObjectDisposedException(nameof(Database<T>));
            if(!isInitialized) throw new DatabaseUninitializedException();

            return new DatabaseEditor<T>(processor);
        }

        public IDatabaseQuery<T> Query()
        {
            if(disposed) throw new ObjectDisposedException(nameof(Database<T>));
            if(!isInitialized) throw new DatabaseUninitializedException();

            return new DatabaseQuery<T>(processor);
        }

        public void Wipe()
        {
            if(disposed) throw new ObjectDisposedException(nameof(Database<T>));
            if(!isInitialized) throw new DatabaseUninitializedException();

            
        }

        public void Dispose()
        {
            disposed = true;
            isInitialized = false;
            Directory = null;
            
            if(processor != null)
                processor.Dispose();
            processor = null;
        }
    }
}