using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Exceptions;
using PBFramework.Debugging;

namespace PBFramework.Data
{
    /// <summary>
    /// A special type of dictionary which can have many keys.
    /// Each key in a keyset must be unique.
    /// Each keyset must have a unique name.
    /// All keys must be non-volatile.
    /// 
    /// Limited to string-type keys only.
    /// </summary>
    public class MultiKeyTable<T> : IMultiKeyTable<T> where T : class {

        /// <summary>
        /// List of all values
        /// </summary>
        private List<T> values = new List<T>();

        /// <summary>
        /// All keysets mapped to their keyset name.
        /// </summary>
        private Dictionary<string, Keyset> keysets = new Dictionary<string, Keyset>();


        public int KeysetCount => keysets.Count;

        public int ValueCount => values.Count;

        public int Count => values.Count;

        public bool IsReadOnly => false;


        public T Get(string setName, string key)
        {
            if (keysets.TryGetValue(setName, out Keyset value))
                return value[key];

            Logger.LogWarning($"MultiKeyTable.Get - Keyset not found for setName: {setName}");
            return null;
        }

        public bool Contains(string setName, string key)
        {
            if(keysets.TryGetValue(setName, out Keyset value))
                return value[key] != null;
            return false;
        }

        public void AddKeyset(string setName, Func<T, string> keySelector)
        {
            if(string.IsNullOrEmpty(setName))
                throw new ArgumentException($"setName mustn't be null or empty!");
            if(keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (keysets.ContainsKey(setName))
                throw new DuplicateKeyException(setName);

            Keyset keyset = new Keyset(keySelector);
            keysets.Add(setName, keyset);
            RefillKeyset(keyset);
        }

        public void Add(T item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));

            values.Add(item);
            AddToKeyset(item);
        }

        public void Clear()
        {
            values.Clear();
            RebuildKeysets();
        }

        public bool Contains(T item) => values.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => values.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (values.Remove(item))
            {
                RemoveFromKeyset(item);
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        /// <summary>
        /// Creates keyset entries for specified item.
        /// </summary>
        protected void AddToKeyset(T item)
        {
            foreach (var keyset in keysets.Values)
                keyset.AddItem(item);
        }

        /// <summary>
        /// Removes all keyset entries generated for specified item.
        /// </summary>
        protected void RemoveFromKeyset(T item)
        {
            foreach(var keyset in keysets.Values)
                keyset.RemoveItem(item);
        }

        /// <summary>
        /// Clears all entries in specified keyset and refills its values.
        /// </summary>
        protected void RefillKeyset(Keyset keyset)
        {
            keyset.Clear();
            values.ForEach(v => keyset.AddItem(v));
        }

        /// <summary>
        /// Rebuilds all keysets for current state.
        /// </summary>
        protected void RebuildKeysets()
        {
            foreach (var keyset in keysets.Values)
                RefillKeyset(keyset);
        }


        /// <summary>
        /// Set of keys that uniquely map to each of its value.
        /// </summary>
        protected class Keyset
        {
            /// <summary>
            /// Function which selects the key from instance T.
            /// </summary>
            private Func<T, string> keySelector;

            /// <summary>
            /// Dictionary of all values mapped to its key.
            /// </summary>
            private Dictionary<string, T> table = new Dictionary<string, T>();


            public T this[string key]
            {
                get
                {
                    if(table.TryGetValue(key, out T value))
                        return value;
                    return null;
                }
            }


            public Keyset(Func<T, string> keySelector)
            {
                this.keySelector = keySelector;
            }

            /// <summary>
            /// Adds specified item to this keyset.
            /// </summary>
            public void AddItem(T item)
            {
                var key = keySelector.Invoke(item);
                table.Add(key, item);
            }

            /// <summary>
            /// Removes specified item from this keyset.
            /// </summary>
            public void RemoveItem(T item)
            {
                table.Remove(keySelector.Invoke(item));
            }

            /// <summary>
            /// Clears the table.
            /// </summary>
            public void Clear() => table.Clear();
        }
    }
}