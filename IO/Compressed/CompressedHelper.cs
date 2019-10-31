using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.IO.Compressed
{
    public static class CompressedHelper {

        /// <summary>
        /// Returns the ICompressed interface for the specified file.
        /// </summary>
        public static ICompressed GetCompressed(FileInfo file)
        {
            switch (file.Extension.ToLowerInvariant())
            {
                case ".zip": return new ZipCompressed(file);
            }
            Logger.LogWarning($"CompressedHelper.GetCompressed - Unknown extension type: ({file.Extension})");
            return new DefaultCompressed(file);
        }
    }
}