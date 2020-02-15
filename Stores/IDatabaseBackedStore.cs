using PBFramework.DB;
using PBFramework.DB.Entities;

namespace PBFramework.Stores
{
    public interface IDatabaseBackedStore<T>
        where T : class, IDatabaseEntity, new()
    {
        /// <summary>
        /// Returns the database instance.
        /// </summary>
        IDatabase<T> Database { get; }


        /// <summary>
        /// Loads the database from the disk.
        /// Any uncommited changes would be lost.
        /// </summary>
        void Reload();
    }
}