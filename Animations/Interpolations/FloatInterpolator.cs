using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Animations.Interpolations
{
    /// <summary>
    /// Handles interpolation of float values.
    /// </summary>
    public class FloatInterpolator : IInterpolator<float> {

        public static FloatInterpolator Instance { get; } = new FloatInterpolator();


        public float Interpolate(float from, float to, float t)
        {
            return (to - from) * t + from;
        }
    }
}