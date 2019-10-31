using System;

namespace PBFramework.Data.Bindables
{
    public class BindableBool : Bindable<bool>
    {
        public BindableBool() : this(default) { }

        public BindableBool(bool value) : base(value) {}

        public override void Parse(string value)
        {
            if(bool.TryParse(value, out bool result))
                Value = result;
        }
    }
}