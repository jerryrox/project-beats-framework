using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Exceptions;

namespace PBFramework.Dependencies
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InitWithDependencyAttribute : Attribute
    {
        private static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Whether null dependency passing is allowed.
        /// </summary>
        public bool AllowNulls { get; set; }


        public InitWithDependencyAttribute(bool allowNulls = false)
        {
            AllowNulls = allowNulls;
        }

        /// <summary>
        /// Returns the injection handler for method with this attribute on specified type.
        /// </summary>
        public static TypeInjector.InjectionHandler GetActivator(Type type)
        {
            var methods = type.GetMethods(Flags).Where(m => m.GetCustomAttribute<InitWithDependencyAttribute>() != null).ToArray();

            // The injection for this attribute only supports 1 method.
            if(methods.Length == 1)
            {
                var method = methods[0];
                var allowNulls = method.GetCustomAttribute<InitWithDependencyAttribute>().AllowNulls;
                var parameters = method.GetParameters().Select(p => p.ParameterType).Select(t => GetParameter(t, type, allowNulls));

                return (obj, container) => {
                    try
                    {
                        method.Invoke(obj, parameters.Select(p => p(container)).ToArray());
                    }
                    catch(TargetInvocationException e)
                    {
                        throw new InitWithDependencyFailedException(type, e.InnerException);
                    }
                };
            }
            return delegate {};
        }

        private static Func<IDependencyContainer, object> GetParameter(Type type, Type requestingType, bool allowNulls)
        {
            return (IDependencyContainer container) => {
                var param = container.Get(type);
                if(param == null && !allowNulls)
                    throw new DependencyNotCachedException(type, requestingType);
                return param;
            };
        }
    }
}
