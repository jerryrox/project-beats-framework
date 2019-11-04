using System;
using System.Collections.Generic;

namespace PBFramework.Data
{
    public interface IMultiKeyTable<T> : ICollection<T>{

        /// <summary>
        /// Returns the total number of keysets currently registered.
        /// </summary>
        int KeysetCount { get; }

        /// <summary>
        /// Returns the value of the "Count" property.
        /// Added for semantics.
        /// </summary>
        int ValueCount { get; }


        /// <summary>
        /// Returns the value associated with specified key under set name.
        /// </summary>
        T Get(string setName, string key);

        /// <summary>
        /// Returns whether an entry exists for specified setname and key.
        /// </summary>
        bool Contains(string setName, string key);

        /// <summary>
        /// Adds a new keyset for specified set name and key selector function.
        /// </summary>
        void AddKeyset(string setName, Func<T, string> keySelector);


    }
}