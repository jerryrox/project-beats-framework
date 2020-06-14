using System.IO;
using System.Collections.Generic;

namespace PBFramework.IO
{
    public interface IHasFiles {
    
        /// <summary>
        /// Returns the list of all files.
        /// </summary>
        List<FileInfo> Files { get; }
    }

    public static class HasFilesExtension
    {
        /// <summary>
        /// Refreshes all files in the list.
        /// </summary>
        public static void RefreshAllFiles(this IHasFiles context)
        {
            if(context.Files != null)
                context.Files.ForEach(f => f.Refresh());
        }
    }
}