using System;

namespace PBFramework.Exceptions
{
    public class DependencyNotCachedException : Exception
    {
        public DependencyNotCachedException(Type type) : base(string.Format("Dependency instance not cached for type of ({0})", type.Name)) { }

        public DependencyNotCachedException(Type type, Type requestingType) : base(string.Format("Dependency instance not " +
            "cached for type of ({0}). Requesting type: ({1})", type.Name, requestingType.Name))
        { }
    }
}

