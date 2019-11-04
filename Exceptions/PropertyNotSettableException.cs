using System;

namespace PBFramework.Exceptions
{
    public class PropertyNotSettableException : Exception
    {
        public PropertyNotSettableException(Type type, string name) : base(string.Format("Property ({0}) from type ({1}) " +
            "can not be written to!", name, type.Name))
        { }
    }
}