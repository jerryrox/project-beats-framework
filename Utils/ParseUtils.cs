using System;

namespace PBFramework.Utils
{
	/// <summary>
	/// Utility class for parsing values with more options.
	/// </summary>
	public static class ParseUtils {
		
		/// <summary>
		/// Parses int value for specified string.
		/// </summary>
		public static int ParseInt(string value, int defaultValue = 0)
		{
            if(int.TryParse(value, out int i))
                return i;
            return defaultValue;
		}

		/// <summary>
		/// Parses float value for specified string.
		/// </summary>
		public static float ParseFloat(string value, float defaultValue = 0f)
		{
            if(float.TryParse(value, out float f))
                return f;
            return defaultValue;
		}

		/// <summary>
		/// Parses double value for specified string.
		/// </summary>
		public static double ParseDouble(string value, double defaultValue = 0f)
		{
            if(double.TryParse(value, out double d))
                return d;
            return defaultValue;
		}
	}
}

