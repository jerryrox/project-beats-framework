using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Allocation.Caching
{
    /// <summary>
    /// Representation of a cached data stored in Cacher object.
    /// </summary>
    public class CachedData<T> {

        /// <summary>
        /// Number of consumers currently using this data.
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// The actual value being cached.
        /// </summary>
        public T Value { get; set; }
    }
}