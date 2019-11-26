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
        /// Returns a default compressed object if returnDefault is true and the specified file was not detectible.
        /// </summary>
        public static ICompressed GetCompressed(FileInfo file, bool returnDefault = false)
        {
            switch (file.Extension.ToLowerInvariant())
            {
                case ".zip":
                case ".osz":
                    return new ZipCompressed(file);
            }
            Logger.LogWarning($"CompressedHelper.GetCompressed - Unknown extension type: ({file.Extension})");
            if(returnDefault)
                return new DefaultCompressed(file);
            return null;
        }
    }
}