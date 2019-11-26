using System;
using System.IO;

namespace PBFramework.Exceptions
{
    public class NotImportableException : Exception {
    
        public NotImportableException(FileInfo file, Type importerType) : base(
            $"The file ({file.Name}) was not importable by ({importerType.Name})."
        ) {}
    }
}