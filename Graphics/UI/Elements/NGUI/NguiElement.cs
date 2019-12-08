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


        public virtual bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
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
        where T : MonoBehaviour
    {
        /// <summary>
        /// The backing component under this element.
        /// </summary>
        protected T component;


        public override bool IsEnabled
        {
            get => base.IsEnabled;
            set => component.enabled = base.IsEnabled = value;
        }


        protected override void Awake()
        {
            base.Awake();
            component = myObject.AddComponent<T>();
        }
    }
}