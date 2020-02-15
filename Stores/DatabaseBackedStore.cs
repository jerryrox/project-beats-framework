using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.DB;
using PBFramework.DB.Entities;

namespace PBFramework.Stores
{
    /// <summary>
    /// Store backed with database.
    /// </summary>
    public abstract class DatabaseBackedStore<T> : IDatabaseBackedStore<T>
        where T : class, IDatabaseEntity, new()
    {
        public IDatabase<T> Database { get; private set; }


        public void Reload()
        {
            // Reload the db.
            if(Database != null)
                Database.Dispose();
            Database = CreateDatabase();
        }

        /// <summary>
        /// Creates a new instance of the database.
        /// </summary>
        protected abstract IDatabase<T> CreateDatabase();
    }
}