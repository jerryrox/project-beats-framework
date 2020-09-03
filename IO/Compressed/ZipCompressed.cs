using System;
using System.IO;
using PBFramework.Threading.Futures;
using PBFramework.Debugging;
using ICSharpCode.SharpZipLib.Zip;

namespace PBFramework.IO.Compressed
{
    /// <summary>
    /// ZIP compression file.
    /// </summary>
    public class ZipCompressed : ICompressed {

        private const int ProgressInterval = 256;

        public FileInfo Source { get; private set; }


        public ZipCompressed(FileInfo file)
        {
            if(file == null)
                throw new InvalidDataException($"ZipCompressed - file mustn't be null!");
            Source = file;
        }

        public long GetUncompressedSize()
        {
            if(!Source.Exists)
                return 0;
            var zip = new ZipFile(Source.FullName);
            long size = 0;
            foreach (ZipEntry entry in zip)
            {
                if(entry.IsFile)
                    size += entry.Size;
            }
            return size;
        }

        public IFuture<DirectoryInfo> Uncompress(DirectoryInfo destination)
        {
            return new AsyncFuture<DirectoryInfo>((future) =>
            {
                try
                {
                    if (destination == null)
                        throw new Exception("Destination is null!");
                    if (!Source.Exists)
                        throw new Exception("Source file does not exist!");

                    using(var fs = new FileStream(Source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Find total size of the zip first.
                        float totalSize = GetUncompressedSize();
                        // Start unzipping.
                        using(var zis = new ZipInputStream(fs))
                        {
                            string destPath = destination.FullName;
                            ZipEntry entry;
                            byte[] buffer = new byte[4096];
                            float curSize = 0;
                            int curInterval = ProgressInterval;
                            while((entry = zis.GetNextEntry()) != null)
                            {
                                string path = Path.Combine(destPath, entry.Name);
                                if(entry.IsDirectory)
                                    Directory.CreateDirectory(path);
                                else
                                {
                                    // Create missing subdirectories.
                                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                                    // Start write for this entry.
                                    using(FileStream writer = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                                    {
                                        int length;
                                        while ((length = zis.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            writer.Write(buffer, 0, length);

                                            // Track progress and report.
                                            curSize += length;
                                            curInterval--;
                                            if (curInterval <= 0)
                                            {
                                                curInterval = ProgressInterval;
                                                future.SetProgress(curSize / totalSize);
                                            }
                                        }
                                        writer.Flush();
                                    }
                                }
                            }
                            // Finished.
                            future.SetProgress(1f);
                        }
                    }
                    future.SetComplete(destination);
                }
                catch (Exception e)
                {
                    Logger.LogError($"ZipCompressed.Uncompress - Error: {e.Message}");
                    future.SetFail(e);
                }
            });
        }
    }
}