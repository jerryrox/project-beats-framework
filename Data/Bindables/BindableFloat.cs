using System;
using PBFramework.Utils;

namespace PBFramework.Data.Bindables
{
    public class BindableFloat : BindableNumber<float>
    {
        public BindableFloat() : this(default) { }

        public BindableFloat(float value) : base(value, float.MinValue, float.MaxValue) {}

        public BindableFloat(float value, float min, float max) : base(value, min, max) {}

        public override void Parse(string value)
        {
            if(float.TryParse(value, out float result))
                Value = result;
        }

        protected override bool EqualTo(float x, float y) => MathUtils.AlmostEquals(x, y);

        protected override bool GreaterThan(float x, float y) => x > y;

        protected override bool LessThan(float x, float y) => x < y;
    }
}
