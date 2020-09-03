using System;
using System.IO;
using PBFramework.Threading.Futures;

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

        public IFuture<DirectoryInfo> Uncompress(DirectoryInfo destination) => new Future<DirectoryInfo>();
    }
}