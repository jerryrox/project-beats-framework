using System;
using System.IO;
using System.Security.Cryptography;

namespace PBFramework.Utils
{
    public static class FileUtils {
    
		/// <summary>
		/// Returns an MD5 hash of specified file.
		/// </summary>
		public static string GetHash(FileInfo file)
		{
			using(var md5 = MD5.Create())
			{
				using(var stream = file.OpenRead())
				{
					var hash = md5.ComputeHash(stream);
					return BitConverter.ToString(hash);
				}
			}
		}
    }
}