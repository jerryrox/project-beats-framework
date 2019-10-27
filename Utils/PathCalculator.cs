using System;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Utils
{
	/// <summary>
	/// Class which provides path calculation methods.
	/// Retrieved from: https://github.com/ppy/osu-framework/blob/e23d64676a2629a1e7856a6fad2966cfe963bfef/osu.Framework/MathUtils/PathApproximator.cs
	/// </summary>
	public static class PathCalculator {

		private const float BezierTolerance = 0.25f;
		private const int CatmullDetail = 50;
		private const float CircularArcTolerance = 0.1f;


		public static List<Vector2> CalculateBezier(Vector2[] points)
		{
			List<Vector2> output = new List<Vector2>();
			int count = points.Length;

			if(count == 0)
				return output;

			var subdivisionBuffer1 = new Vector2[count];
			var subdivisionBuffer2 = new Vector2[count * 2 - 1];

			Stack<Vector2[]> toFlatten = new Stack<Vector2[]>();
			Stack<Vector2[]> freeBuffers = new Stack<Vector2[]>();
			toFlatten.Push(points);

			var leftChild = subdivisionBuffer2;
			while(toFlatten.Count > 0)
			{
				Vector2[] parent = toFlatten.Pop();
				if(IsBezierFlatEnough(parent))
				{
					ApproximateBezier(parent, output, subdivisionBuffer1, subdivisionBuffer2, count);
					freeBuffers.Push(parent);
					continue;
				}

				// If we do not yet have a sufficiently "flat" (in other words, detailed) approximation we keep
				// subdividing the curve we are currently operating on.
				Vector2[] rightChild = freeBuffers.Count > 0 ? freeBuffers.Pop() : new Vector2[count];
				SubdivideBezier(parent, leftChild, rightChild, subdivisionBuffer1, count);

				// We re-use the buffer of the parent for one of the children, so that we save one allocation per iteration.
				for (int i=0; i<count; i++)
					parent[i] = leftChild[i];

				toFlatten.Push(rightChild);
				toFlatten.Push(parent);
			}

			output.Add(points[count - 1]);
			return output;
		}

		public static List<Vector2> CalculateCatmull(Vector2[] points)
		{
			var result = new List<Vector2>((points.Length - 1) * CatmullDetail * 2);

			for(int i=0; i<points.Length-1; i++)
			{
				var v1 = i > 0 ? points[i - 1] : points[i];
				var v2 = points[i];
				var v3 = i < points.Length - 1 ? points[i + 1] : v2 + v2 - v1;
				var v4 = i < points.Length - 2 ? points[i + 2] : v3 + v3 - v2;

				for(int c=0; c<CatmullDetail; c++)
				{
					result.Add(CatmullFindPoint(ref v1, ref v2, ref v3, ref v4, (float)c / CatmullDetail));
					result.Add(CatmullFindPoint(ref v1, ref v2, ref v3, ref v4, (float)(c + 1) / CatmullDetail));
				}
			}

			return result;
		}

		public static List<Vector2> ApproximateCircularArc(Vector2[] points)
		{
			Vector2 a = points[0];
			Vector2 b = points[1];
			Vector2 c = points[2];

			float aSq = (b - c).sqrMagnitude;
			float bSq = (a - c).sqrMagnitude;
			float cSq = (a - b).sqrMagnitude;

			if (MathUtils.AlmostEquals(aSq, 0) || MathUtils.AlmostEquals(bSq, 0) || MathUtils.AlmostEquals(cSq, 0))
				return new List<Vector2>();

			float s = aSq * (bSq + cSq - aSq);
			float t = bSq * (aSq + cSq - bSq);
			float u = cSq * (aSq + bSq - cSq);
			float sum = s + t + u;

			if (MathUtils.AlmostEquals(sum, 0))
				return new List<Vector2>();

			Vector2 centre = (s * a + t * b + u * c) / sum;
			Vector2 dA = a - centre;
			Vector2 dC = c - centre;

			float r = dA.magnitude;

			double thetaStart = Math.Atan2(dA.y, dA.x);
			double thetaEnd = Math.Atan2(dC.y, dC.x);

			while (thetaEnd < thetaStart)
				thetaEnd += 2 * Math.PI;

			double dir = 1;
			double thetaRange = thetaEnd - thetaStart;

			Vector2 orthoAtoC = c - a;
			orthoAtoC = new Vector2(orthoAtoC.y, -orthoAtoC.x);
			if (Vector2.Dot(orthoAtoC, b - a) < 0)
			{
				dir = -dir;
				thetaRange = 2 * Math.PI - thetaRange;
			}

			int amountPoints = 2 * r <= CircularArcTolerance ? 2 : Math.Max(2, (int)Math.Ceiling(thetaRange / (2 * Math.Acos(1 - CircularArcTolerance / r))));

			List<Vector2> output = new List<Vector2>(amountPoints);

			for (int i = 0; i < amountPoints; ++i)
			{
				double fract = (double)i / (amountPoints - 1);
				double theta = thetaStart + dir * fract * thetaRange;
				Vector2 o = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * r;
				output.Add(centre + o);
			}

			return output;
		}

		public static List<Vector2> ApproximateLinear(Vector2[] points)
		{
			var result = new List<Vector2>(points.Length);

			foreach(var c in points)
				result.Add(c);

			return result;
		}

		public static List<Vector2> ApproximateLagrangePolynomial(Vector2[] points)
		{
			int stepCount = 51;
			var result = new List<Vector2>(stepCount);

			double[] weights = Interpolation.BarycentricWeights(points);

			float minX = points[0].x;
			float maxX = points[0].x;
			for (int i = 1; i < points.Length; i++)
			{
				minX = Math.Min(minX, points[i].x);
				maxX = Math.Max(maxX, points[i].x);
			}
			float dx = maxX - minX;

			for (int i = 0; i < stepCount; i++)
			{
				float x = minX + dx / (stepCount - 1) * i;
				float y = (float)Interpolation.BarycentricLagrange(points, weights, x);
				result.Add(new Vector2(x, y));
			}

			return result;
		}

		private static bool IsBezierFlatEnough(Vector2[] points)
		{
			float sqrTolerance = BezierTolerance * BezierTolerance * 4;
			for(int i=1; i<points.Length-1; i++)
			{
				if((points[i-1] - points[i]*2 + points[i+1]).sqrMagnitude > sqrTolerance)
					return false;
			}
			return true;
		}

		private static void SubdivideBezier(Vector2[] points, Vector2[] l, Vector2[] r, Vector2[] subdivisionBuffer, int count)
		{
			var midPoints = subdivisionBuffer;

			for(int i=0; i<count; i++)
				midPoints[i] = points[i];

			for(int i=0; i<count; i++)
			{
				l[i] = midPoints[0];
				r[count-i-1] = midPoints[count-i-1];

				for(int c=0; c<count-i-1; c++)
				{
					midPoints[c] = (midPoints[c] + midPoints[c+1]) / 2;
				}
			}
		}

		private static void ApproximateBezier(Vector2[] points, List<Vector2> output, Vector2[] subdivisionBuffer1, Vector2[] subdivisionBuffer2, int count)
		{
			var l = subdivisionBuffer2;
			var r = subdivisionBuffer1;

			SubdivideBezier(points, l, r, subdivisionBuffer1, count);

			for(int i=0; i<count-1; i++)
				l[count+i] = r[i+1];

			output.Add(points[0]);
			for(int i=1; i<count-1; i++)
			{
				int index = i * 2;
				output.Add((l[index-1] + l[index]*2 + l[index+1]) * 0.25f);
			}
		}

		private static Vector2 CatmullFindPoint(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, ref Vector2 v4, float t)
		{
			float t2 = t * t;
			float t3 = t * t2;
			return new Vector2(
				0.5f * (2f * v2.x + (-v1.x + v3.x) * t + (2f * v1.x - 5f * v2.x + 4f * v3.x - v4.x) * t2 + (-v1.x + 3f * v2.x - 3f * v3.x + v4.x) * t3),
				0.5f * (2f * v2.y + (-v1.y + v3.y) * t + (2f * v1.y - 5f * v2.y + 4f * v3.y - v4.y) * t2 + (-v1.y + 3f * v2.y - 3f * v3.y + v4.y) * t3)
			);
		}
	}
}

