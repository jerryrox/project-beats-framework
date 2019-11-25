using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Utils
{
    public static class IOUtils {

        /// <summary>
        /// Copies the specified directory 'from' to 'to' directory.
        /// </summary>
        public static void CopyDirectory(DirectoryInfo from, DirectoryInfo to)
        {
            if(from == null) throw new ArgumentNullException(nameof(from));
            if(to == null) throw new ArgumentNullException(nameof(to));
            
            Action<DirectoryInfo, DirectoryInfo> copyAction = null;
            copyAction = (s, t) =>
            {
                Directory.CreateDirectory(t.FullName);

                // Copy each file into the new directory.
                foreach (FileInfo file in s.GetFiles())
                {
                    file.CopyTo(Path.Combine(t.FullName, file.Name), true);
                }
                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo sourceSub in s.GetDirectories())
                {
                    DirectoryInfo targetSub = t.CreateSubdirectory(sourceSub.Name);
                    copyAction(sourceSub, targetSub);
                }
            };
            copyAction(from, to);
        }
    }
}