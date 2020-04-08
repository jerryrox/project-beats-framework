using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data.Bindables
{
    public class ProxyBindableInt : ProxyBindableNumber<int> {
    
        public ProxyBindableInt(Func<int> getter, Action<int> setter) : base(getter, setter, int.MinValue, int.MaxValue) {}

        public ProxyBindableInt(Func<int> getter, Action<int> setter, int min, int max) : base(getter, setter, min, max) {}

        protected override bool EqualTo(int x, int y) => x == y;

        protected override bool GreaterThan(int x, int y) => x > y;

        protected override bool LessThan(int x, int y) => x < y;
    }
}