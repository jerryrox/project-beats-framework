using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.DB
{
    public class DatabaseInitializedException : Exception {
    
        public DatabaseInitializedException() : base(
            $"The database has already been initialized."
        ) { }
    }
}