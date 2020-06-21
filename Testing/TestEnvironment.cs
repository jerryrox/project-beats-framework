using System;
using System.Linq;
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

            // Key-bound action automated testing
            bool useManualKeyTests = false;
            if (testOptions.KeyAction != null)
            {
                var keyBindings = testOptions.KeyAction.KeyBindings;
                if (keyBindings != null && keyBindings.Length > 0)
                {
                    LogInformation(true, "Running key-bound test actions automatically.");

                    // Toggle manual test
                    useManualKeyTests = testOptions.KeyAction.UseManualTesting;

                    // Run automated key action tests.
                    foreach (var key in keyBindings)
                    {
                        var action = key.RunAction(false);
                        if(action != null)
                            yield return action;
                    }


                    if (useManualKeyTests)
                    {
                        // Output all the available keys for manual testing.
                        LogInformation(false, "[LeftControl+1] : Success");
                        LogInformation(false, "[LeftControl+2] : Fail");
                        LogInformation(
                            false,
                            keyBindings.Select(k => k.GetUsage()).ToArray()
                        );
                        // Manual test must be done during update lifecycle so it shouldn't be null.
                        if(testOptions.UpdateMethod == null)
                            testOptions.UpdateMethod = () => { };
                    }
                }
            }

            // Update lifecycle method
            if (testOptions.UpdateMethod != null)
            {
                while (IsRunning)
                {
                    // Run manual key action tests.
                    if (useManualKeyTests)
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (Input.GetKeyDown(KeyCode.Alpha1))
                            {
                                EndSuccess();
                                continue;
                            }
                            if (Input.GetKeyDown(KeyCode.Alpha2))
                            {
                                EndFail("Manually failed test");
                                continue;
                            }
                        }

                        foreach (var key in testOptions.KeyAction.KeyBindings)
                        {
                            var action = key.RunAction(true);
                            if (action != null)
                            {
                                LogInformation(true, $"Executing manual test ({key.Description})");
                                yield return action;
                                LogInformation(false, "Test ended.");
                            }
                        }
                    }

                    testOptions.UpdateMethod.Invoke();
                    yield return null;
                }
            }
            else
            {
                EndSuccess();
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

        /// <summary>
        /// Logs the specified information to the console.
        /// </summary>
        private void LogInformation(bool addHeader, params string[] information)
        {
            if(addHeader)
                Debug.LogWarning("===============================");
            foreach(string info in information)
                Debug.Log(info);
        }
    }
}