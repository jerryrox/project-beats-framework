using System;
using System.IO;

namespace PBFramework.Utils
{
	/// <summary>
	/// Utility class which provides helper functions for working with file system.
	/// </summary>
	public static class PathUtils {

		/// <summary>
		/// Returns the standardized path of specified path by using only '/' as a directory separator.
		/// </summary>
		public static string StandardPath(string path)
		{
			return path.Replace('\\', '/');
		}

		/// <summary>
		/// Returns the file:// path for local requests for given path.
		/// </summary>
		public static string LocalRequestPath(string path)
		{
			#if UNITY_ANDROID || UNITY_IPHONE
			return "file://"+StandardPath(path);
			#else
			return "file:///"+AccessiblePath(path);
			#endif
		}

		/// <summary>
		/// Returns the converted path where all of its separator characters are standardized to its system preference.
		/// </summary>
		public static string NativePath(string path)
		{
			return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar);
		}
	}
}

