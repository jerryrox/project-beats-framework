using System;
using UnityEngine;
using PBFramework.Data;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    public class UguiObject : MonoBehaviour, IGraphicObject {

        protected GameObject myObject;
        protected RectTransform myTransform;

        private UguiObject parent;
        private SortedList<UguiObject> children;

        private Pivots pivot = Pivots.Center;
        private Anchors anchor = Anchors.Center;
        private int depth = 0;

        private IDependencyContainer dependencies;


        public string Name
        {
            get => name;
            set => name = value;
        }

        public GameObject RawObject => myObject;

        public IGraphicObject Parent => parent;

        public RectTransform RawTransform => myTransform;

        public float X
        {
            get => myTransform.localPosition.x;
            set => myTransform.SetLocalPositionX(value);
        }

        public float Y
        {
            get => myTransform.localPosition.y;
            set => myTransform.SetLocalPositionY(value);
        }

        public float Width
        {
            get => myTransform.sizeDelta.x;
            set => myTransform.SetSizeX(value);
        }

        public float Height
        {
            get => myTransform.sizeDelta.y;
            set => myTransform.SetSizeY(value);
        }

        public Vector2 Position
        {
            get => myTransform.localPosition;
            set => myTransform.localPosition = value;
        }

        public Vector2 Size
        {
            get => myTransform.sizeDelta;
            set => myTransform.sizeDelta = value;
        }

        public float RotationZ
        {
            get => myTransform.localEulerAngles.z;
            set => myTransform.SetLocalEulerZ(value);
        }

        public Vector3 Rotation
        {
            get => myTransform.localEulerAngles;
            set => myTransform.localEulerAngles = value;
        }

        public float ScaleX
        {
            get => myTransform.localScale.x;
            set => myTransform.SetLocalScaleX(value);
        }

        public float ScaleY
        {
            get => myTransform.localScale.y;
            set => myTransform.SetLocalScaleY(value);
        }

        public Vector3 Scale
        {
            get => myTransform.localScale;
            set => myTransform.localScale = value;
        }

        public Pivots Pivot
        {
            get => pivot;
            set => myTransform.SetPivot(pivot = value);
        }

        public Anchors Anchor
        {
            get => anchor;
            set => myTransform.SetAnchor(anchor = value);
        }

        public int Depth
        {
            get => depth;
            set
            {
                depth = value;
                if (parent != null)
                {
                    parent.children.Remove(this);
                    parent.children.Add(this);
                    parent.ReorderChildren();
                }
            }
        }


        protected virtual void Awake()
        {
            myObject = gameObject;
            myTransform = GetComponent<RectTransform>();

            children = new SortedList<UguiObject>();
        }

        public virtual void ResetSize() {}

        public IGraphicObject CreateChild(string name = "")
        {
            return CreateChild<UguiObject>(name);
        }

        public virtual T CreateChild<T>(string name = "") where T : MonoBehaviour, IGraphicObject
        {
            var child = new GameObject(name).AddComponent<T>();
            child.SetParent(this);
            dependencies?.Inject(child);
            return child;
        }

        public void SetParent(IGraphicObject parent)
        {
            if(parent == null) throw new ArgumentNullException(nameof(parent));
            if(parent.Equals(this)) throw new ArgumentException($"parent mustn't be itself!");
            if(!(parent is UguiObject graphicParent)) throw new ArgumentException($"parent must be a type of {nameof(UguiObject)}");

            // Move out from existing parent.
            this.parent?.children.Remove(this);

            // Move in to new parent.
            graphicParent.children.Add(this);

            // Replace parent reference.
            this.parent = graphicParent;

            // Set parent in transform component.
            myTransform.SetParent(graphicParent.RawTransform);

            // Re-order the siblings in the parent object.
            this.parent.ReorderChildren();
        }

        public virtual void Destroy()
        {
            // Remove the child from parent's children list.
            parent?.children.Remove(this);
            // Destroy the actual gameobject.
            GameObject.Destroy(myObject);
        }

        public int CompareTo(IGraphicObject other)
        {
            return depth.CompareTo(other.Depth);
        }

        /// <summary>
        /// Re-orders children based on their depth.
        /// </summary>
        private void ReorderChildren()
        {
            // Apply the order in hierarchy tree.
            for (int i = 0; i < children.Count; i++)
            {
                children[i].myTransform.SetSiblingIndex(i);
            }
        }
    }

    /// <summary>
    /// Generic implementation of GraphicObject which assumes encapsulated component to provide its function.
    /// </summary>
    public abstract class UguiObject<T> : UguiObject
        where T : MonoBehaviour
    {
        protected T component;


        protected override void Awake()
        {
            base.Awake();
            component = myObject.AddComponent<T>();
        }
    }
}