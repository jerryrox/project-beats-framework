using System;

namespace PBFramework.Utils
{
	/// <summary>
	/// Math utility clas for Game framework.
	/// </summary>
	public static class MathUtils {

		public const float FloatEpsilon = 1e-3f;
		public const double DoubleEpsilon = 1e-7;

		/// <summary>
		/// Returns the clamped value between specified min and max range.
		/// </summary>
		public static double Clamp(double value, double min, double max)
		{
			if(value < min)
				return min;
			if(value > max)
				return max;
			return value;
		}

		/// <summary>
		/// Returns the value of linear-interpolant t between min and max ranges.
		/// </summary>
		public static double Lerp(double min, double max, double t)
		{
			return (max - min) * t + min;
		}

		/// <summary>
		/// Returns the interpolant t value between specified range.
		/// </summary>
		public static double InverseLerp(double min, double max, double value)
		{
			return (value - min) / (max - min);
		}

		/// <summary>
		/// Returns whether two float values are almost equal.
		/// </summary>
		public static bool AlmostEquals(float f1, float f2, float maxDifference = FloatEpsilon)
		{
			return Math.Abs(f1 - f2) <= maxDifference;
		}

		/// <summary>
		/// Returns whether two double values are almost equal.
		/// </summary>
		public static bool AlmostEquals(double d1, double d2, double maxDifference = DoubleEpsilon)
		{
			return Math.Abs(d1 - d2) <= maxDifference;
		}
	}
}

