using System;

namespace PBFramework.Exceptions
{
    public class ImmutableException : Exception {

        public ImmutableException() : base(
            $"The state of this object is not mutable!"
        ) {}

        public ImmutableException(string reason) : base(
            $"The state of this object is not mutable! Reason: ({reason})"
        ) {}
    }
}