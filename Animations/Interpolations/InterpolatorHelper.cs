using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Animations.Interpolations
{
    public static class InterpolatorHelper {

        /// <summary>
        /// Returns the interpolator instance for specified type T.
        /// </summary>
        public static IInterpolator<T> GetInterpolator<T>()
        {
            if(typeof(T) == typeof(int))
                return IntInterpolator.Instance as IInterpolator<T>;
            else if(typeof(T) == typeof(float))
                return FloatInterpolator.Instance as IInterpolator<T>;

            throw new Exception($"Unknown type ({nameof(T)}) specified.");
        }
    }
}