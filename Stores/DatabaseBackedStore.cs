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
    public abstract class DatabaseBackedStore<T>
        where T : class, IDatabaseEntity, new()
    {
        protected readonly IDatabase<T> database;


        protected DatabaseBackedStore(IDatabase<T> database)
        {
            if(database == null) throw new ArgumentNullException(nameof(database));

            this.database = database;
        }
    }
}