using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.Dependencies
{
    public class DependencyContainer : IDependencyContainer
    {
        /// <summary>
        /// Table of dependencies mapped to a unique type.
        /// </summary>
        private Dictionary<Type, object> dependencies = new Dictionary<Type, object>();


        public DependencyContainer(bool cacheSelf = true)
        {
            if(cacheSelf)
                CacheAs<IDependencyContainer>(this);
        }

        public void CacheAndInject(object value)
        {
            Cache(value);
            Inject(value);
        }

        public void CacheAndInjectAs<T>(T value) where T : class
        {
            CacheAs<T>(value);
            Inject(value);
        }

        public void Cache(object value, bool replace = false)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));

            CacheAs(value.GetType(), value, replace);
        }

        public void CacheAs<T>(T value, bool replace = false) where T : class
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));

            CacheAs(typeof(T), value, replace);
        }

        public void CacheFrom(IDependencyContainer container, bool replace = false)
        {
            if(container == null)
                throw new ArgumentNullException(nameof(container));

            foreach(var dependency in container.GetDependencies())
            {
                CacheAs(dependency.Key, dependency.Value, replace);
            }
        }

        public IDependencyContainer Clone()
        {
            var container = new DependencyContainer(false);
            container.CacheFrom(this);
            if(container.Get<IDependencyContainer>() != null)
                container.CacheAs<IDependencyContainer>(container, true);
            return container;
        }

        public bool Contains<T>() => dependencies.ContainsKey(typeof(T));

        public T Get<T>()
        {
            if(dependencies.TryGetValue(typeof(T), out object value))
                return (T)value;

            Logger.LogWarning($"DependencyContainer.Get - Failed to find dependency of type: {typeof(T).Name}");
            return default;
        }

        public object Get(Type type)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));

            if(dependencies.TryGetValue(type, out object value))
                return value;

            Logger.LogWarning($"DependencyContainer.Get - Failed to find dependency of type: {type.Name}");
            return null;
        }

        public void Inject(object obj)
        {
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));

            DependencyInjector.Inject(obj, this);
        }

        public void Remove<T>() => dependencies.Remove(typeof(T));

        public void Remove(object value)
        {
            if(value == null)
                throw new ArgumentNullException(nameof(value));

            dependencies.Remove(value.GetType());
        }

        public IEnumerable<KeyValuePair<Type, object>> GetDependencies() => dependencies;

        /// <summary>
        /// Caches the specified object by mapping it to given type.
        /// </summary>
        private void CacheAs(Type type, object value, bool replace = false)
        {
            if(dependencies.ContainsKey(type) && !replace)
            {
                Logger.LogWarning($"DependencyContainer.CacheAs - A dependency already exists for type ({type.Name})!");
                return;
            }
            dependencies[type] = value;
        }
    }
}
