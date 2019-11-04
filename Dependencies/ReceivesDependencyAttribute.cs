using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using PBFramework.Exceptions;

namespace PBFramework.Dependencies
{
    /// <summary>
    /// Attribute which allows target properties to receive dependency instances through injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReceivesDependencyAttribute : Attribute
    {
        private static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Whether nulls values can be passed to the target property.
        /// </summary>
        private bool allowNulls;


        public ReceivesDependencyAttribute(bool allowNulls = false)
        {
            this.allowNulls = allowNulls;
        }

        public static TypeInjector.InjectionHandler GetActivator(Type type)
        {
            var activators = new List<Action<object, IDependencyContainer>>();
            var properties = type.GetProperties(Flags).Where(p => p.GetCustomAttribute<ReceivesDependencyAttribute>() != null);

            foreach(var property in properties)
            {
                if(!property.CanWrite)
                    throw new PropertyNotSettableException(type, property.Name);

                var attribute = property.GetCustomAttribute<ReceivesDependencyAttribute>();
                var propertyType = property.PropertyType;
                var getDependency = GetDependency(propertyType, type, attribute.allowNulls);

                activators.Add((target, container) => {
                    try
                    {
                        var value = getDependency(container);
                        property.SetValue(target, value ?? Convert.ChangeType(value, propertyType), null);
                    }
                    catch(Exception e)
                    {
                        throw new ReceivesDependencyFailedException(type, property, e);
                    }
                });
            }

            return (obj, container) => activators.ForEach(a => a.Invoke(obj, container));
        }

        private static Func<IDependencyContainer, object> GetDependency(Type type, Type requestingType, bool allowNulls)
        {
            return (IDependencyContainer container) => {
                var dependency = container.Get(type);
                if(dependency == null && !allowNulls)
                    throw new DependencyNotCachedException(type, requestingType);
                return dependency;
            };
        }
    }
}

