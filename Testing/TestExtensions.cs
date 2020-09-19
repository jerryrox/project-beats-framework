using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Testing
{
    /// <summary>
    /// A class that provides extensions useful for testing.
    /// </summary>
    public static class TestExtensions {

        /// <summary>
        /// Returns the child located at the specified path, while retrieving the component of to type T.
        /// </summary>
        public static T FindFromPath<T>(this MonoBehaviour context, string path)
            where T : Component
        {
            return context.transform.Find(path)?.GetComponent<T>();
        }

        /// <summary>
        /// Returns the child of specified component and name.
        /// </summary>
        public static T FindWithName<T>(this MonoBehaviour context, string name)
            where T : Component
        {
            var children = context.GetComponentsInChildren<T>(true);
            return children?.FirstOrDefault(c => c.name.Equals(name));
        }
    }
}