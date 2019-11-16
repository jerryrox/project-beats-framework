using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.DB
{
    public class DatabaseUninitializedException : Exception {
    
        public DatabaseUninitializedException() : base(
            $"The database has not been initialized yet."
        ) { }
    }
}