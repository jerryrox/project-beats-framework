using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Testing
{
    public class KeyActionOptions {

        /// <summary>
        /// Whether the TestEnvironment should also run in manual testing mode.
        /// </summary>
        public bool UseManualTesting { get; set; }

        /// <summary>
        /// Test action bindings to be automated and/or performed manually.
        /// </summary>
        public TestKeyBinding[] KeyBindings { get; set; }
    }
}