using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PBFramework.DB.Entities;

namespace PBFramework.DB
{
    public class DatabaseResult<T> : IDatabaseResult<T>
        where T : class, IDatabaseEntity, new()
    {
        private bool disposed = false;
        private IEnumerator<T> results;

        private int count;
        private int index;
        private T current;


        public T Current
        {
            get
            {
                if(disposed) throw new ObjectDisposedException(nameof(DatabaseResult<T>));
                if(index >= count) throw new EndOfCursorException();
                return current;
            }
        }
        object IEnumerator.Current => Current;

        public int Count => count;


        public DatabaseResult(IEnumerator<T> results, int count)
        {
            this.results = results;

            this.count = count;
            this.index = -1;
            this.current = null;
        }

        public bool MoveNext()
        {
            if(disposed) throw new ObjectDisposedException(nameof(DatabaseResult<T>));

            index++;
            if (index < count && results.MoveNext())
            {
                current = results.Current;
                return true;
            }
            return false;
        }

        public void Reset() => throw new NotSupportedException();

        public IEnumerator<T> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;

        public void Dispose()
        {
            disposed = true;
            results = null;
            count = 0;
            index = 0;
            current = null;
        }
    }
}