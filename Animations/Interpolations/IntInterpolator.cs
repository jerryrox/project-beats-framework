using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Animations.Interpolations
{
    public class IntInterpolator : IInterpolator<int> {
    
        public static IntInterpolator Instance { get; } = new IntInterpolator();


        public int Interpolate(int from, int to, float t)
        {
            return (int)((to - from) * t) + from;
        }
    }
}