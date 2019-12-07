using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Base implementation of all elements.
    /// </summary>
    public abstract class NguiElement<T> : MonoBehaviour, IElement
        where T : Component
    {
        protected GameObject myObject;

        protected Transform myTransform;

        /// <summary>
        /// Cached display instance.
        /// </summary>
        protected IDisplay display;

        /// <summary>
        /// The backing component under this element.
        /// </summary>
        protected T component;


        public bool IsEnabled
        {
            get => enabled;
            set => display.enabled = enabled = value;
        }

        public GameObject RawObject => myObject;

        public Transform Transform => myTransform;

        public IDisplay Display
        {
            get => display;
            set => display = value;
        }


        protected virtual void Awake()
        {
            myObject = gameObject;
            myTransform = transform;
            component = myObject.AddComponent<T>();
        }
    }
}