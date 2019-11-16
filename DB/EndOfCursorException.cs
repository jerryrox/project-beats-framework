using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.DB
{
    public class EndOfCursorException : Exception {
    
        public EndOfCursorException() : base(
            "Attempted to access next entry when the cursor has already reached its end."
        ) {}
    }
}