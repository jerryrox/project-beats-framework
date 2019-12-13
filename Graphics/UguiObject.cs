using System;
using UnityEngine;
using PBFramework.Data;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    [RequireComponent(typeof(RectTransform))]
    public class UguiObject : MonoBehaviour, IGraphicObject {

        protected GameObject myObject;
        protected RectTransform myTransform;

        private UguiObject parent;
        private SortedList<UguiObject> children;

        private Pivots pivot = Pivots.Center;
        private Anchors anchor = Anchors.Center;
        private int depth = 0;


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

        public Vector2 Position
        {
            get => myTransform.localPosition;
            set => myTransform.localPosition = value;
        }

        public float Width
        {
            get => FromSizeDeltaX(myTransform.sizeDelta.x);
            set => myTransform.SetSizeDeltaX(ToSizeDeltaX(value));
        }

        public float Height
        {
            get => FromSizeDeltaY(myTransform.sizeDelta.y);
            set => myTransform.SetSizeDeltaY(ToSizeDeltaY(value));
        }

        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.x;
                Height = value.y;
            }
        }

        public float RawWidth
        {
            get => myTransform.sizeDelta.x;
            set => myTransform.SetSizeDeltaX(value);
        }

        public float RawHeight
        {
            get => myTransform.sizeDelta.y;
            set => myTransform.SetSizeDeltaY(value);
        }

        public Vector2 RawSize
        {
            get => myTransform.sizeDelta;
            set => myTransform.sizeDelta = value;
        }

        public float RotationX
        {
            get => myTransform.localEulerAngles.x;
            set => myTransform.SetLocalEulerX(value);
        }

        public float RotationY
        {
            get => myTransform.localEulerAngles.y;
            set => myTransform.SetLocalEulerY(value);
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

        [ReceivesDependency]
        protected IDependencyContainer Dependency { get; private set; }


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
            // Assign parent
            child.SetParent(this);

            // Reset properties
            child.gameObject.layer = myObject.layer;
            child.transform.ResetTransform();

            // Inject dependencies
            Dependency?.Inject(child);
            return child;
        }

        public virtual T AddComponentInject<T>() where T : MonoBehaviour
        {
            var component = myObject.AddComponent<T>();
            if (component is UguiObject uguiComponent)
            {
                uguiComponent.parent = this.parent;
            }
            Dependency?.Inject(component);
            return component;
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
        /// Converts the specified size delta x value to absolute size.
        /// </summary>
        protected float FromSizeDeltaX(float value)
        {
            if(parent != null && ((anchor >= Anchors.TopStretch && anchor <= Anchors.BottomStretch) || anchor == Anchors.Fill))
                return value + parent.Width;
            return value;
        }

        /// <summary>
        /// Converts the specified size delta y value to absolute size.
        /// </summary>
        protected float FromSizeDeltaY(float value)
        {
            if(parent != null && ((anchor >= Anchors.LeftStretch && anchor <= Anchors.RightStretch) || anchor == Anchors.Fill))
                return value + parent.Height;
            return value;
        }

        /// <summary>
        /// Converts the spacified absolute size x value to size delta.
        /// </summary>
        protected float ToSizeDeltaX(float value)
        {
            if(parent != null && ((anchor >= Anchors.TopStretch && anchor <= Anchors.BottomStretch) || anchor == Anchors.Fill))
                return value - parent.Width;
            return value;
        }

        /// <summary>
        /// Converts the specified absolute size y value to size delta.
        /// </summary>
        protected float ToSizeDeltaY(float value)
        {
            if(parent != null && ((anchor >= Anchors.LeftStretch && anchor <= Anchors.RightStretch) || anchor == Anchors.Fill))
                return value - parent.Height;
            return value;
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