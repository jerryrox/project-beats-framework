using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Services
{
    /// <summary>
    /// Provides access to Unity-specific features which can't be accessed from classes other than MonoBehaviour type.
    /// </summary>
    public class UnityThreadService : MonoBehaviour {

        private static UnityThreadService I;
        private static MonoBehaviour MI;


        /// <summary>
        /// Returns the instance of this class.
        /// </summary>
        private static UnityThreadService Instance => I == null ? I = new GameObject().AddComponent<UnityThreadService>() : I;

        /// <summary>
        /// Returns the instance of this class as MonoBehaviour type.
        /// </summary>
        private static MonoBehaviour MonoInstance => MI == null ? MI = Instance : MI;


        /// <summary>
        /// Starts a new coroutine on the unity thread.
        /// </summary>
        public static new Coroutine StartCoroutine(IEnumerator enumerator) => MonoInstance.StartCoroutine(enumerator);

        /// <summary>
        /// Stops the specified coroutine if currently running.
        /// </summary>
        public static new void StopCoroutine(Coroutine coroutine) => MonoInstance.StopCoroutine(coroutine);
    }
}