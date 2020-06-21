using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBFramework.Testing
{
    /// <summary>
    /// Provides a temporary environment to test certain things within the UnityTest process.
    /// </summary>
    public class TestEnvironment : MonoBehaviour {

        private TestOptions testOptions;

        private bool isEnded;


        /// <summary>
        /// Returns whether the environment is currently running.
        /// </summary>
        public bool IsRunning => !isEnded;


        /// <summary>
        /// Initializes a new test environment for a UnityTest process.
        /// </summary>
        public static TestEnvironment Setup<T>(T runner, TestOptions testOptions)
            where T : class
        {
            if(runner == null)
                throw new ArgumentNullException(nameof(runner));
            if(testOptions == null)
                throw new ArgumentNullException(nameof(testOptions));

            var env = new GameObject("_TestEnvironment").AddComponent<TestEnvironment>();
            env.SetupInternal(runner, testOptions);
            return env;
        }

        /// <summary>
        /// Runs the test environment and returns an enumerator to return from the test method.
        /// </summary>
        public IEnumerator Run()
        {
            if(!IsRunning)
                yield break;

            // Update lifecycle method
            if (testOptions.UpdateMethod != null)
            {
                while (IsRunning)
                {
                    testOptions.UpdateMethod.Invoke();
                    yield return null;
                }
            }
            else
            {
                EndSuccess();
                // Wait for a frame to have consistent behavior.
                yield return null;
            }

            // Cleanup lifecycle method
            if(testOptions.CleanupMethod != null)
                testOptions.CleanupMethod.Invoke();
        }

        /// <summary>
        /// Stops the running environment with a successful result.
        /// </summary>
        public void EndSuccess()
        {
            if(!IsRunning)
                return;
            isEnded = true;
        }

        /// <summary>
        /// Stops the running environment with a failed result.
        /// </summary>
        public void EndFail(string reason)
        {
            if(!IsRunning)
                return;
            throw new Exception($"[Test manually failed] : {reason}");
        }

        /// <summary>
        /// Internally handles the Setup process.
        /// </summary>
        private void SetupInternal<T>(T runner, TestOptions testOptions)
            where T : class
        {
            this.testOptions = testOptions;

            if(testOptions.Dependency == null)
                testOptions.Dependency = new DependencyContainer(true);

            // Initialize camera
            if (testOptions.UseCamera)
            {
                Camera camera = new GameObject("_Camera").AddComponent<Camera>();
                camera.tag = "MainCamera";
                camera.gameObject.AddComponent<AudioListener>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.black;
            }

            // Initialize default root.
            if (testOptions.DefaultRoot != null)
            {
                IRoot root = UguiRoot.Create(testOptions.Dependency);
                if(Camera.main != null)
                    root.SetCameraRender(Camera.main);
                else
                    root.SetOverlayRender();
                root.BaseResolution = testOptions.DefaultRoot.BaseResolution;
            }

            // Inititialize
            testOptions.Dependency.Inject(runner);
        }
    }
}