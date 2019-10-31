using System;
using PBFramework.Utils;

namespace PBFramework.Data.Bindables
{
    public class BindableDouble : BindableNumber<double>
    {
        public BindableDouble() : this(default) { }

        public BindableDouble(double value) : base(value, double.MinValue, double.MaxValue) { }

        public BindableDouble(double value, double min, double max) : base(value, min, max) { }

        public override void Parse(string value)
        {
            if(double.TryParse(value, out double result))
                Value = result;
        }

        protected override bool EqualTo(double x, double y) => MathUtils.AlmostEquals(x, y);

        protected override bool GreaterThan(double x, double y) => x > y;

        protected override bool LessThan(double x, double y) => x < y;
    }
}
