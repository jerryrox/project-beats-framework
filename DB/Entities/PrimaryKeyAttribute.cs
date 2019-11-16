using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.DB.Entities
{
    /// <summary>
    /// Assigns the target variable as the primary key.
    /// Usually, this would be attached on Guid type properties.
    /// There must always be only one primary key attribute in any entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : IndexedAttribute {
    
        public PrimaryKeyAttribute()
        {
        }
    }
}