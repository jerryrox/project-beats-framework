using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Graphics.UI.Elements.NGUI
{
    /// <summary>
    /// Base implementation of all elements.
    /// </summary>
    public abstract class NguiElement : MonoBehaviour, IElement
    {
        protected GameObject myObject;

        protected Transform myTransform;

        /// <summary>
        /// Cached display instance.
        /// </summary>
        protected IDisplay display;


        public bool IsEnabled
        {
            get => enabled;
            set => display.IsEnabled = enabled = value;
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
        }
    }

    /// <summary>
    /// Generic implementation of NguiElement where the component T is automatically added for you.
    /// </summary>
    public abstract class NguiElement<T> : NguiElement
        where T : Component
    {
        /// <summary>
        /// The backing component under this element.
        /// </summary>
        protected T component;


        protected override void Awake()
        {
            base.Awake();
            component = myObject.AddComponent<T>();
        }
    }
}