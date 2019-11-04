using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data
{
    /// <summary>
    /// Extension of MultiKeyTable which assumes implementation of IHasIdentifier on object T.
    /// </summary>
    public class IdMultiKeyTable<T> : MultiKeyTable<T> where T : class, IHasIdentifier {
    
        public IdMultiKeyTable()
        {
            // Add default keyset "Id".
            AddKeyset("Id", value => value.Id.ToString());
        }
    }
}