using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    using ReceiverList = PBFramework.Data.SortedList<IInputReceiver>;

    public class InputManager : MonoBehaviour, IInputManager {

        private bool useMouse;
        private bool useTouch;
        private bool useKeyboard;
        private bool useAcceleration;

        private MouseCursor[] mouseCursors;
        private TouchCursor[] touchCursors;
        private List<KeyboardKey> keyboardKeys;
        private IAccelerator accelerator;

        private uint touchUpdateId = 0;

        private ReceiverList receivers;


        public int MaxMouseCount { get; private set; }

        public int MaxTouchCount { get; private set; }

        public IAccelerator Accelerator
        {
            get => accelerator;
            set => accelerator = value;
        }

        public bool UseMouse
        {
            get => useMouse;
            set => UseInput(mouseCursors, value, ref useMouse);
        }

        public bool UseTouch
        {
            get => useTouch;
            set => UseInput(touchCursors, value, ref useTouch);
        }

        public bool UseKeyboard
        {
            get => useKeyboard;
            set => UseInput(keyboardKeys, value, ref useKeyboard);
        }

        public bool UseAcceleration
        {
            get => useAcceleration;
            set => useAcceleration = value;
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

        public void SetResolution(Vector2 resolution)
        {
            foreach (var mouse in mouseCursors)
                mouse.SetResolution(resolution);
            foreach (var touch in touchCursors)
                touch.SetResolution(resolution);
        }

        public void AddReceiver(IInputReceiver receiver)
        {
            // Prepare for input sorting due to receiver addition
            for (int i = 0; i < receivers.Count; i++)
                receivers[i].PrepareInputSort();
            receiver.PrepareInputSort();

            // Add the receiver.
            receivers.Add(receiver);
        }

        public void RemoveReceiver(IInputReceiver receiver)
        {
            receivers.Remove(receiver);
        }

        public IKey AddKey(KeyCode keyCode)
        {
            var key = FindKey(keyCode);
            if(key != null)
            {
                key.Listeners++;
                return key;
            }
            // If not already exist, register a new keycode
            var newKey = new KeyboardKey(keyCode);
            newKey.SetActive(useKeyboard);
            keyboardKeys.Add(newKey);
            return newKey;
        }

        public void RemoveKey(KeyCode keyCode)
        {
            var key = FindKey(keyCode);
            if(key == null) return;

            key.Release();

            key.Listeners--;
            if (key.Listeners <= 0)
            {
                key.SetActive(false);
                keyboardKeys.Remove(key);
            }
        }

        public ICursor GetMouse(int index) => mouseCursors[index];

        public ICursor GetTouch(int index) => touchCursors[index];

        public IKey GetKey(KeyCode keyCode) => FindKey(keyCode);

        public IEnumerable<ICursor> GetMouses() => mouseCursors;

        public IEnumerable<ICursor> GetTouches() => touchCursors;

        public IEnumerable<ICursor> GetCursors()
        {
            foreach(var m in mouseCursors)
                yield return m;
            foreach (var t in touchCursors)
                yield return t;
        }

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

            receivers = new ReceiverList(8);

            // Use inputs by default.
            UseMouse = UseTouch = UseKeyboard = true;

            SetResolution(resolution);
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
        /// Finds and returns the KeyboardKey instance associated with the specified keycode.
        /// </summary>
        private KeyboardKey FindKey(KeyCode keyCode) => keyboardKeys.Where(key => key.Key == keyCode).FirstOrDefault();

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

            if (useAcceleration && accelerator != null)
            {
                accelerator.Update();
            }

            if (useKeyboard)
            {
                for (int i = keyboardKeys.Count - 1; i >= 0; i--)
                    keyboardKeys[i].Process();
            }

            // Process input receivers.
            for (int i = 0; i < receivers.Count; i++)
            {
                if(!receivers[i].ProcessInput())
                    break;
            }
        }
    }
}