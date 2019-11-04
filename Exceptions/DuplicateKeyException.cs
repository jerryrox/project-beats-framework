using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Exceptions
{
    public class DuplicateKeyException : Exception {
    
        public DuplicateKeyException(string keyName) : base(
            string.Format("A duplicate key was registered for: ({0})", keyName)
        ){}
    }
}