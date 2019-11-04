using System;
using System.Reflection;

namespace PBFramework.Exceptions
{
    public class ReceivesDependencyFailedException : Exception
    {
        public ReceivesDependencyFailedException(Type type, PropertyInfo property, Exception innerException) : base(
            string.Format(
                "Failed to inject dependency into property for type ({0}), property ({1}).\n" +
                "Inner Exception: ({2}) with trace: {3}",
                type.Name,
                property.Name,
                innerException == null ? "null" : innerException.Message,
                innerException == null ? "null" : innerException.StackTrace
            ),
            innerException
        )
        { }
    }
}