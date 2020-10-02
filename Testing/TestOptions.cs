using System;
using PBFramework.Dependencies;

namespace PBFramework.Testing
{
    public class TestOptions {

        /// <summary>
        /// The dependencies to be injected to the test class.
        /// This is also used to inject dependencies in root object.
        /// </summary>
        public IDependencyContainer Dependency { get; set; }

        /// <summary>
        /// Whether the test environment should initialize a camera for a visual feedback.
        /// </summary>
        public bool UseCamera { get; set; }

        /// <summary>
        /// Actions to be executed within the test environment.
        /// </summary>
        public TestAction[] Actions { get; set; }

        /// <summary>
        /// Options for default root.
        /// Specify a non-null value to activate this option.
        /// </summary>
        public DefaultRootOptions DefaultRoot { get; set; }

        /// <summary>
        /// An action to be invoked for Update process.
        /// </summary>
        public Action UpdateMethod { get; set; }

        /// <summary>
        /// An action to be invoked for test cleanup.
        /// </summary>
        public Action CleanupMethod { get; set; }
    }
}