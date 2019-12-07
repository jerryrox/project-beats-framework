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

        public void Inject(IDependencyContainer container)
        {
            if(container == null) throw new ArgumentNullException(nameof(container));
            this.dependencies = container;
            container.Inject(this);
        }

        public virtual IObject CreateChild()
        {
            var obj = new GameObject("child").AddComponent<GObject>();
            obj.SetParent(this);
            obj.Inject(dependencies);
            return obj;
        }

        public virtual void AddChild(IObject child)
        {
            child.SetParent(this);
            child.Inject(dependencies);
        }

        public virtual void AddChildren(IEnumerable<IObject> children)
        {
            foreach (var child in children)
                AddChild(child);
        }

        public virtual void SetParent(Transform transform) => myTransform.SetParent(transform);

        public virtual void SetParent(IObject obj) => myTransform.SetParent(obj.Transform);

        public T AddComponent<T>() where T : Component => myObject.AddComponent<T>();

        public new T GetComponent<T>() where T : Component => base.GetComponent<T>();

        public new T GetComponentInChildren<T>() where T : Component => base.GetComponentInChildren<T>();

        public new T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component => base.GetComponentsInChildren<T>(includeInactive);

        public new T GetComponentInParent<T>() where T : Component => base.GetComponentInParent<T>();

        public virtual void Destroy() => Destroy(myObject);
    }
}