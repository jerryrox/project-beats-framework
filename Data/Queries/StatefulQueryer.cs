using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data.Queries
{
    public abstract class StatefulQueryer<T> : Queryer<T>, IStatefulQueryer<T> where T : class, IQueryableData {

        public abstract IEnumerable<T> All { get; }


        public IEnumerable<T> Query(string queryString = "")
        {
            return Query(All, queryString);
        }
    }
}