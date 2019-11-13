using System;
using System.IO;
using System.Threading.Tasks;

namespace PBFramework.IO.Compressed
{
    /// <summary>
    /// Interface for accessing or interacting with a compressed file.
    /// </summary>
    public interface ICompressed {

        /// <summary>
        /// Returns the FileInfo which represents the compressed file.
        /// </summary>
        FileInfo Source { get; }


        /// <summary>
        /// Returns the size of the files when the file is uncompressed.
        /// </summary>
        /// <returns></returns>
        long GetUncompressedSize();

        /// <summary>
        /// Uncompresses the compressed file to the specified destination.
        /// If successful, returns the same directory provided. Otherwise, null is returned.
        /// </summary>
        Task<DirectoryInfo> Uncompress(DirectoryInfo destination, IProgress<float> progress = null);
    }
}