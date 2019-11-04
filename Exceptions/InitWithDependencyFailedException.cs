using System;

namespace PBFramework.Exceptions
{
    public class InitWithDependencyFailedException : Exception
    {
        public InitWithDependencyFailedException(Type type, Exception innerException) : base(
            string.Format(
                "Failed to inject dependencies through method invocation for type ({0}).\n" +
                "Inner Exception: ({1}) with trace: {2}",
                type.Name,
                innerException == null ? "null" : innerException.Message,
                innerException == null ? "null" : innerException.StackTrace
            ),
            innerException
        )
        { }
    }
}