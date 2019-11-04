using System;
using System.Collections.Generic;

namespace PBFramework.Dependencies
{
    public interface IDependencyContainer
    {
        /// <summary>
        /// Returns the cached dependency of type T.
        /// </summary>
        T Get<T>();

        /// <summary>
        /// Returns the cached dependency of specified type.
        /// </summary>
        object Get(Type type);

        /// <summary>
        /// Caches the specified dependency instance as its top-most child type.
        /// </summary>
        void Cache(object value, bool replace = false);

        /// <summary>
        /// Caches the specified dependency instance as specified type T.
        /// </summary>
        void CacheAs<T>(T value, bool replace = false) where T : class;

        /// <summary>
        /// Caches all dependencies from specified container.
        /// </summary>
        void CacheFrom(IDependencyContainer container, bool replace = false);

        /// <summary>
        /// Removes the dependency instance of specified type T.
        /// </summary>
        void Remove<T>();

        /// <summary>
        /// Removes the specified dependency instance.
        /// </summary>
        void Remove(object value);

        /// <summary>
        /// Injects dependencies onto specified object.
        /// </summary>
        void Inject<T>(T obj) where T : class;

        /// <summary>
        /// Clones this container.
        /// </summary>
        IDependencyContainer Clone();

        /// <summary>
        /// Returns all dependencies cached in this object.
        /// </summary>
        IEnumerable<KeyValuePair<Type, object>> GetDependencies();
    }
}