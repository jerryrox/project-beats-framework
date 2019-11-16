using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.DB.Entities
{
    /// <summary>
    /// Indexes this variable to the index file to be able to query with this variable much faster.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IndexedAttribute : Attribute {
    
        public IndexedAttribute()
        {
        }
    }
}