using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    public class InputManager : MonoBehaviour, IInputManager {

        private bool useMouse;
        private bool useTouch;
        private bool useKeyboard;

        private MouseCursor[] mouseCursors;
        private TouchCursor[] touchCursors;
        private List<KeyboardKey> keyboardKeys;

        private uint touchUpdateId = 0;


        public int MaxMouseCount { get; private set; }

        public int MaxTouchCount { get; private set; }

        public bool UseMouse
        {
            get => useMouse;
            set
            {
                useMouse = value;
                UseInput(mouseCursors, value, ref useMouse);
            }
        }

        public bool UseTouch
        {
            get => useTouch;
            set
            {
                useTouch = value;
                UseInput(touchCursors, value, ref useTouch);
            }
        }

        public bool UseKeyboard
        {
            get => useKeyboard;
            set
            {
                useKeyboard = value;
                UseInput(keyboardKeys, value, ref useKeyboard);
            }
        }


        /// <summary>
        /// Creates a new instance of the InputManager.
        /// </summary>
        public static InputManager Create(Vector2 resolution,
            int maxMouseCursors = 2, int maxTouchCursors = 6)
        {
            if(resolution.x <= 0 || resolution.y <= 0)
                throw new ArgumentException("resolution must be greater than 0!");
            if(maxMouseCursors < 0)
                throw new ArgumentNullException("maxMouseCursors must be 0 or greater!");
            if(maxTouchCursors < 0)
                throw new ArgumentNullException("maxTouchCursors must be 0 or greater!");

            var manager = new GameObject("_InputManager").AddComponent<InputManager>();;
            manager.Initialize(resolution, maxMouseCursors, maxTouchCursors);
            return manager;
        }

        public IKey AddKey(KeyCode keyCode)
        {
            var key = FindKey(keyCode);
            if(key != null) return key;
            // If not already exist, register a new keycode
            var newKey = new KeyboardKey(keyCode);
            newKey.SetActive(useKeyboard);
            keyboardKeys.Add(newKey);
            Debug.Log($"Added new key of code: {keyCode}. Count: {keyboardKeys.Count}");
            return newKey;
        }

        public void RemoveKey(KeyCode keyCode)
        {
            var key = FindKey(keyCode);
            if(key == null) return;
            key.Release();
            key.SetActive(false);
            keyboardKeys.Remove(key as KeyboardKey);
        }

        public ICursor GetMouse(int index) => mouseCursors[index];

        public ICursor GetTouch(int index) => touchCursors[index];

        public IKey GetKey(KeyCode keyCode) => FindKey(keyCode);

        public IEnumerable<ICursor> GetMouses() => mouseCursors;

        public IEnumerable<ICursor> GetTouches() => touchCursors;

        public IEnumerable<IKey> GetKeys() => keyboardKeys;

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        private void Initialize(Vector2 resolution,
            int maxMouseCursors, int maxTouchCursors)
        {
            MaxMouseCount = maxMouseCursors;
            MaxTouchCount = maxTouchCursors;

            mouseCursors = new MouseCursor[maxMouseCursors];
            for (int i = 0; i < mouseCursors.Length; i++)
                mouseCursors[i] = new MouseCursor(KeyCode.Mouse0 + i, resolution);

            touchCursors = new TouchCursor[maxTouchCursors];
            for (int i = 0; i < touchCursors.Length; i++)
                touchCursors[i] = new TouchCursor(KeyCode.Mouse0 + i, resolution);

            keyboardKeys = new List<KeyboardKey>(4);

            // Use inputs by default.
            UseMouse = UseTouch = UseKeyboard = true;
        }

        /// <summary>
        /// Internally toggles use state of a specific input type.
        /// </summary>
        private void UseInput<T>(IEnumerable<T> inputs, bool use, ref bool flag)
            where T : IInput
        {
            flag = use;
            if (!use)
            {
                foreach (var input in inputs)
                {
                    input.Release();
                    input.SetActive(false);
                }
            }
            else
            {
                foreach(var input in inputs)
                    input.SetActive(true);
            }
        }

        /// <summary>
        /// Finds and returns the IKey instance associated with the specified keycode.
        /// </summary>
        private IKey FindKey(KeyCode keyCode) => keyboardKeys.Where(key => key.Key == keyCode).FirstOrDefault();

        private void Update()
        {
            if (useMouse)
            {
                for (int i = 0; i < mouseCursors.Length; i++)
                    mouseCursors[i].Process();
            }

            if (useTouch)
            {
                touchUpdateId++;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if(touch.fingerId >= touchCursors.Length)
                        continue;
                    touchCursors[touch.fingerId].Process(touch, touchUpdateId);
                }
                for (int i = 0; i < touchCursors.Length; i++)
                    touchCursors[i].VerifyTouch(touchUpdateId);
            }
            
            if (useKeyboard)
            {
                for (int i = keyboardKeys.Count - 1; i >= 0; i--)
                    keyboardKeys[i].Process();
            }
        }
    }
}