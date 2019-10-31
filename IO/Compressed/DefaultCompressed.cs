using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PBFramework.IO.Compressed
{
    /// <summary>
    /// Default implementation for ICompressed without any behavior.
    /// </summary>
    public class DefaultCompressed : ICompressed {

        public FileInfo Source { get; private set; }


        public DefaultCompressed(FileInfo file)
        {
            Source = file;
        }

        public long GetUncompressedSize() => 0;

        public Task<DirectoryInfo> Uncompress(DirectoryInfo destination, IProgress<double> progress) => Task.Run(() => null as DirectoryInfo);
    }
}