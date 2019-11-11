using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Networking
{
    /// <summary>
    /// Extension class with context of Networking-related features.
    /// </summary>
    public static class NetworkingExtensions {

		/// <summary>
		/// Parses response code from this dictionary.
		/// The dictionary must represent a collection of response headers.
		/// Returns 0 if any invalid condition is met.
		/// </summary>
		public static long GetResponseCode(this Dictionary<string,string> context) {
			if(context == null)
				return 0;
			if(!context.ContainsKey("STATUS"))
				return 0;
			
			string[] components = context["STATUS"].Split(' ');
			if (components.Length < 3)
				return 0;

			long ret = 0;
			if (!long.TryParse(components[1], out ret))
				return 0;
			return ret;
		}
    
		/// <summary>
		/// Returns the uri-escaped representation of this string.
		/// </summary>
		public static string GetUriEscaped(this string context, bool unescapeFirst = true) {
			if(unescapeFirst) {
				string decoded = Uri.UnescapeDataString(context);
				if(!decoded.Equals(context, StringComparison.Ordinal))
					return context;
			}
			return Uri.EscapeUriString(context);
		}
    }
}