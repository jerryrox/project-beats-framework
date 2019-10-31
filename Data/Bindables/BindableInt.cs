using System;

namespace PBFramework.Data.Bindables
{
    public class BindableInt : BindableNumber<int>
    {
        public BindableInt() : this(default) { }

        public BindableInt(int value) : base(value, int.MinValue, int.MaxValue) {}

        public BindableInt(int value, int min, int max) : base(value, min, max) {}

        public override void Parse(string value)
        {
            if(int.TryParse(value, out int result))
                Value = result;
        }

        protected override bool EqualTo(int x, int y) => x == y;

        protected override bool GreaterThan(int x, int y) => x > y;

        protected override bool LessThan(int x, int y) => x < y;
    }
}