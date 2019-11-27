using PBFramework.IO;
using PBFramework.DB.Entities;
using PBFramework.Data;

namespace PBFramework.Stores
{
    /// <summary>
    /// Indicates that the object is eligible for storing managed by a DirectoryBackedStore.
    /// </summary>
    public interface IDirectoryIndex : IDatabaseEntity, IHashable, IHasDirectory {
    
    }
}