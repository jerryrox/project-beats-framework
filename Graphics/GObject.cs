using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Default IObject implementation.
    /// </summary>
    public class GObject : MonoBehaviour, IObject {

        protected GameObject myObject;

        protected Transform myTransform;

        protected IDependencyContainer dependencies;


        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        public GameObject RawObject => myObject;

        public Transform Transform => myTransform;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool IsActive
        {
            get => myObject.activeInHierarchy;
            set => myObject.SetActive(value);
        }


        protected virtual void Awake()
        {
            this.myObject = gameObject;
            this.myTransform = transform;
        }

        public virtual IObject CreateChild()
        {
            var obj = new GameObject("child").AddComponent<GObject>();
            obj.SetParent(this);
            dependencies?.Inject(obj);
            return obj;
        }

        public virtual void AddChild(IObject child)
        {
            child.SetParent(this);
            dependencies?.Inject(child);
        }

        public virtual void AddChildren(IEnumerable<IObject> children)
        {
            foreach (var child in children)
                AddChild(child);
        }

        public virtual void SetParent(Transform transform) => myTransform.SetParent(transform);

        public virtual void SetParent(IObject obj) => myTransform.SetParent(obj.Transform);

        public virtual T AddComponent<T>() where T : Component
        {
            var component = myObject.AddComponent<T>();
            dependencies?.Inject(component);
            return component;
        }

        public virtual void Destroy() => Destroy(myObject);
    }
}