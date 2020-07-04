using System;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.Dependencies
{
    /// <summary>
    /// Contains the necessary information required to inject dependencies for a single type.
    /// </summary>
    public class TypeInjector
    {
        /// <summary>
        /// The object type which the injector is responsible for.
        /// </summary>
        private Type type;

        /// <summary>
        /// The injector for the base type of the responsible type.
        /// </summary>
        private TypeInjector baseInjector;

        /// <summary>
        /// List of all handlers cached for the responsible type.
        /// </summary>
        private List<InjectionHandler> handlers = new List<InjectionHandler>();

        /// <summary>
        /// Whether the injector was already initialized.
        /// </summary>
        private bool isInitialized;


        /// <summary>
        /// Returns the base injector instance if exists.
        /// </summary>
        public TypeInjector BaseInjector => baseInjector;


        /// <summary>
        /// Delegate for handling injection.
        /// </summary>
        public delegate void InjectionHandler(object obj, IDependencyContainer container);


        public TypeInjector(Type type) : this(type, null) { }

        public TypeInjector(Type type, TypeInjector baseInjector)
        {
            this.type = type;
            this.baseInjector = baseInjector;
        }

        /// <summary>
        /// Initializes the injector.
        /// </summary>
        public void Initialize()
        {
            if(isInitialized)
                return;

            handlers.Add(ReceivesDependencyAttribute.GetActivator(type));
            handlers.Add(InitWithDependencyAttribute.GetActivator(type));

            isInitialized = true;
        }

        /// <summary>
        /// Performs injection on specified object.
        /// </summary>
        public void Inject(object obj, IDependencyContainer container)
        {
            // Make sure the types are matching.
            if(type != obj.GetType())
            {
                Logger.LogError($"TypeInjector.Inject - Injection target's type ({obj.GetType().Name}) does not match the responsible type ({type.Name})!");
                return;
            }

            InjectInternal(obj, container);
        }

        /// <summary>
        /// Internally handles injection process.
        /// </summary>
        private void InjectInternal(object obj, IDependencyContainer container)
        {
            // Ensure that it's initialized first.
            Initialize();

            // Call base injector's injection first.
            baseInjector?.InjectInternal(obj, container);

            // Inject.
            handlers.ForEach(h => h.Invoke(obj, container));
        }
    }
}
