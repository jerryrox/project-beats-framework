using System;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Animations.Interpolations
{
    public static class InterpolatorHelper {

        private static Dictionary<Type, object> Interpolators = new Dictionary<Type, object>()
        {
            { typeof(int), IntInterpolator.Instance },
            { typeof(float), FloatInterpolator.Instance },
            { typeof(Vector2), Vector2Interpolator.Instance },
            { typeof(Vector3), Vector3Interpolator.Instance },
            { typeof(Color), ColorInterpolator.Instance },
        };


        /// <summary>
        /// Returns the interpolator instance for specified type T.
        /// </summary>
        public static IInterpolator<T> GetInterpolator<T>()
        {
            if (Interpolators.TryGetValue(typeof(T), out object interpolator))
                return interpolator as IInterpolator<T>;

            throw new Exception($"Unknown type ({nameof(T)}) specified.");
        }
    }
}