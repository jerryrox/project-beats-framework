using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Utils;

namespace PBFramework.Data.Bindables
{
    public class ProxyBindableFloat : ProxyBindableNumber<float>
    {
        public ProxyBindableFloat(Func<float> getter, Action<float> setter) : base(getter, setter, float.MinValue, float.MaxValue) { }

        public ProxyBindableFloat(Func<float> getter, Action<float> setter, float min, float max) : base(getter, setter, min, max) { }

        protected override bool EqualTo(float x, float y) => MathUtils.AlmostEquals(x, y);

        protected override bool GreaterThan(float x, float y) => x > y;

        protected override bool LessThan(float x, float y) => x < y;
    }
}