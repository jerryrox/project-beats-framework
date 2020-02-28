using System;
using System.Collections.Generic;

namespace PBFramework.Dependencies
{
    /// <summary>
    /// Static helper which performs injection of dependencies onto objects through reflection.
    /// </summary>
    public static class DependencyInjector
    {
        /// <summary>
        /// Holds list of all injection handlers cached for specific object types.
        /// </summary>
        private static Dictionary<Type, TypeInjector> Handlers = new Dictionary<Type, TypeInjector>();


        /// <summary>
        /// Performs injection on specified class-type object.
        /// </summary>
        public static void Inject<T>(T obj, IDependencyContainer container) where T : class
        {
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));
            if(container == null)
                throw new ArgumentNullException(nameof(container));

            TypeInjector injector = GetInjector(typeof(T));
            injector.Inject(obj, container);
        }

        /// <summary>
        /// Returns the injector instance for specified type.
        /// </summary>
        public static TypeInjector GetInjector(Type type)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));

            // Cached?
            if(Handlers.TryGetValue(type, out TypeInjector value))
                return value;

            // Get base type's injector
            TypeInjector baseInjector = null;
            Type baseType = type.BaseType;
            if(baseType != null && baseType != typeof(object))
                baseInjector = GetInjector(baseType);

            // Create new and cache
            value = new TypeInjector(type, baseInjector);
            value.Initialize();
            Handlers.Add(type, value);
            return value;
        }
    }
}
