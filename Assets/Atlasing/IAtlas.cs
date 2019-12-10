using System;

namespace PBFramework.Assets.Atlasing
{
    /// <summary>
    /// Abstraction of an object holding a collection of objects mapped to their resource name.
    /// </summary>
    public interface IAtlas<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Sets the object associated to specified name.
        /// </summary>
        void Set(string name, T obj);

        /// <summary>
        /// Returns the object associated with specified name.
        /// </summary>
        T Get(string name);

        /// <summary>
        /// Whether the object of specified name exists.
        /// </summary>
        bool Contains(string name);
    }
}