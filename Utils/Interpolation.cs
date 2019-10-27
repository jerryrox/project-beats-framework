using System;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Utils
{
	/// <summary>
	/// Utility class which provides interpolation calculation.
	/// Referenced from: https://github.com/ppy/osu-framework/blob/e23d64676a2629a1e7856a6fad2966cfe963bfef/osu.Framework/MathUtils/Interpolation.cs
	/// </summary>
	public static class Interpolation {
		
		public static double BarycentricLagrange(Vector2[] points, double[] weights, double time)
		{
            if (points == null || points.Length == 0)
				throw new ArgumentException("Interpolation.BarycentricLagrange - points must contain at least one point");
			if (points.Length != weights.Length)
				throw new ArgumentException("Interpolation.BarycentricLagrange - points must contain exactly as many items as weights");

			double numerator = 0;
			double denominator = 0;
			for(int i=0; i<points.Length; i++)
			{
				if(time == points[i].x)
					return points[i].y;

				double li = weights[i] / (time - points[i].x);
				numerator += li * points[i].y;
				denominator += li;
			}

			return numerator / denominator;
		}

		public static double[] BarycentricWeights(Vector2[] points)
		{
			int n = points.Length;
			double[] w = new double[n];
			for(int i=0; i<n; i++)
			{
				w[i] = 1;
				for(int j=0; j<n; j++)
				{
					if(i != j)
						w[i] *= points[i].x - points[j].x;
				}
				w[i] = 1.0 / w[i];
			}
			return w;
		}
	}
}

