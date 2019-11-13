using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;
using ICSharpCode.SharpZipLib.Core;
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

        public Task<DirectoryInfo> Uncompress(DirectoryInfo destination, IProgress<float> progress)
        {
            return Task.Run(() => {
                if (destination == null)
                {
                    Logger.LogError($"ZipCompressed.Uncompress - destination is null!");
                    return null;
                }
                if(!Source.Exists)
                    return null;

                // Reset progress
                progress.Report(0);

                try
                {
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
                                                progress.Report(curSize / totalSize);
                                            }
                                        }
                                        writer.Flush();
                                    }
                                }
                            }
                            // Finished.
                            progress.Report(1f);
                        }
                    }
                    return destination;
                }
                catch (Exception e)
                {
                    Logger.LogError($"ZipCompressed.Uncompress - Error: {e.Message}");
                    return null;
                }
            });
        }
    }
}