using System.Collections.Generic;
using UnityEngine;

// for creating the dive arc when hunting for a seed
// code based on https://github.com/shamim-akhtar/bezier-curve
namespace SeedSnatcher.Utils
{
    public static class BezierCurve
    {
        // factorial lookup table
        private static readonly float[] factorial =
        {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
        };

        private static float Binomial(int n, int k)
        {
            // n!/(k!(n-k)!) where 0 <= k <= n
            // fail case: negative numbers & k>n
            if (n < 0 || k < 0 || k > n) return -1f;

            var numerator = factorial[n];
            var denominator = factorial[k] * factorial[n - k];
            return numerator / denominator;
        }

        private static float Bernstein(int n, int k, float p)
        {
            // (n choose k)(x^k)((1-x)^(n-k))
            var binomial = Binomial(n, k);
            // return early if bad data
            if (binomial < 0f || p > 1 || p < 0) return -1;

            var probSuccess = Mathf.Pow(p, k);
            var probFailure = Mathf.Pow(1 - p, n - k);
            return binomial * probSuccess * probFailure;
        }

        public static Vector3 CalculateBezierPoint(List<Vector3> controlPoints, float t)
        {
            var n = controlPoints.Count - 1;
            var curvePosition = Vector3.zero;
            for (var k = 0; k <= n; k++)
            {
                curvePosition += Bernstein(n, k, t) * controlPoints[k];
            }

            return curvePosition;
        }
    }
}