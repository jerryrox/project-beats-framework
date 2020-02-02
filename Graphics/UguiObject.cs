using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Data;
using PBFramework.Inputs;
using PBFramework.Graphics.Effects;
using PBFramework.Dependencies;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Graphics
{
    [RequireComponent(typeof(RectTransform))]
    public class UguiObject : MonoBehaviour, IGraphicObject {

        protected GameObject myObject;
        protected RectTransform myTransform;

        private UguiObject parent;
        private SortedList<IGraphicObject> children = new SortedList<IGraphicObject>();

        /// <summary>
        /// Path which traverses through the ugui hierarchy this object to the root.
        /// </summary>
        private List<UguiObject> inputPath;

        /// <summary>
        /// The table of effects currently applied.
        /// </summary>
        private Lazy<Dictionary<Type, IEffect>> effects;

        private Pivots pivot = Pivots.Center;
        private Anchors anchor = Anchors.Center;
        private bool isInited = false;
        private int depth = 0;


        public string Name
        {
            get => name;
            set => name = value;
        }

        public GameObject RawObject => myObject;

        public IGraphicObject Parent => parent;

        public bool Active
        {
            get => myObject.activeInHierarchy;
            set => myObject.SetActive(value);
        }

        public RectTransform RawTransform => myTransform;

        public float X
        {
            get => myTransform.anchoredPosition3D.x;
            set => myTransform.SetAnchoredPositionX(value);
        }

        public float Y
        {
            get => myTransform.anchoredPosition3D.y;
            set => myTransform.SetAnchoredPositionY(value);
        }

        public float Z
        {
            get => myTransform.anchoredPosition3D.z;
            set => myTransform.SetAnchoredPositionZ(value);
        }

        public Vector3 Position
        {
            get => myTransform.anchoredPosition3D;
            set => myTransform.anchoredPosition3D = value;
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

        public float OffsetLeft
        {
            get => myTransform.offsetMin.x;
            set => myTransform.SetOffsetLeft(value);
        }
        
        public float OffsetRight
        {
            get => myTransform.offsetMax.x;
            set => myTransform.SetOffsetRight(value);
        }
        
        public float OffsetTop
        {
            get => myTransform.offsetMax.y;
            set => myTransform.SetOffsetTop(value);
        }
        
        public float OffsetBottom
        {
            get => myTransform.offsetMin.y;
            set => myTransform.SetOffsetBottom(value);
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
            get => Canvas ? Canvas.sortingOrder : depth;
            set
            {
                if (Canvas == null)
                {
                    depth = value;
                    if (parent != null)
                    {
                        parent.children.Remove(this);
                        parent.children.Add(this);
                        parent.ReorderChildren();
                    }
                }
                else
                    Canvas.sortingOrder = value;
            }
        }

        public int InputLayer => 0;

        /// <summary>
        /// A canvas component cached from this gameobject to detect depth overriding.
        /// </summary>
        public Canvas Canvas { get; set; }

        [ReceivesDependency]
        public IDependencyContainer Dependencies { get; set; }

        /// <summary>
        /// Returns the root graphic object in the hierarchy.
        /// </summary>
        protected IRoot Root => Dependencies?.Get<IRoot>();

        /// <summary>
        /// Returns the input manager instance.
        /// </summary>
        protected IInputManager InputManager => Dependencies?.Get<IInputManager>();


        protected virtual void Awake()
        {
            myObject = gameObject;
            myTransform = GetComponent<RectTransform>();
        }

        public virtual void ResetSize() {}

        public IGraphicObject CreateChild(string name = "", int depth = 0, IDependencyContainer dependencies = null)
        {
            return CreateChild<UguiObject>(name, depth, dependencies);
        }

        public virtual T CreateChild<T>(string name = "", int depth = 0, IDependencyContainer dependencies = null) where T : MonoBehaviour, IGraphicObject
        {
            var child = new GameObject(name).AddComponent<T>();
            // Assign parent
            child.Depth = depth;
            child.SetParent(this);

            // Reset properties
            child.gameObject.layer = myObject.layer;
            child.transform.ResetTransform();

            // Inject dependencies.
            // Prioritize the dependency given as argument.
            (dependencies ?? Dependencies)?.Inject(child);
            return child;
        }

        public virtual T AddComponentInject<T>() where T : MonoBehaviour
        {
            var component = myObject.AddComponent<T>();
            if (component is UguiObject uguiComponent)
            {
                uguiComponent.parent = this.parent;
            }
            Dependencies?.Inject(component);
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

        public T AddEffect<T>(T effect) where T : class, IEffect
        {
            if(effect == null) throw new ArgumentNullException(nameof(effect));

            var effects = this.effects.Value;
            // Multiple materials are not supported due to limitations.
            if (effect.UsesMaterial)
            {
                if (effects.Values.Any(e => e.UsesMaterial))
                {
                    Logger.LogWarning($"UguiObject.AddEffect - Failed to add effect ({nameof(T)}). An existing effect is utilizing the object's material.");
                    return null;
                }
            }
            // Try applying the effect.
            if(!effect.Apply(this)) return null;
            // Inject dependencies.
            Dependencies?.Inject(effect);
            // Add the effect.
            effects.Add(typeof(T), effect);
            return effect;
        }

        public void RemoveEffect<T>() where T : class, IEffect
        {
            var effects = this.effects.Value;
            // Find the effect of specified type and revert its effects first.
            if (effects.TryGetValue(typeof(T), out IEffect effect))
            {
                effect.Revert(this);
                effects.Remove(typeof(T));
            }
        }

        public T GetEffect<T>() where T : class, IEffect
        {
            var effects = this.effects.Value;
            if(effects.TryGetValue(typeof(T), out IEffect effect))
                return (T)effect;
            return null;
        }

        public void SetReceiveInputs(bool listen)
        {
            var inputManager = InputManager;
            if(inputManager == null) return;

            if(listen)
                inputManager.AddReceiver(this);
            else
                inputManager.RemoveReceiver(this);
        }

        public virtual bool ProcessInput() => true;

        void IInputReceiver.PrepareInputSort()
        {
            // Determine the path between this object and its closest canvas object.
            if(inputPath == null)
                inputPath = new List<UguiObject>(4);
            else
                inputPath.Clear();

            var curNode = this;
            do
            {
                // Add current node
                inputPath.Add(curNode);

                // Check whether current node has an overriding canvas
                if (curNode.Canvas != null)
                {
                    // We should break the traversing here, since overridden canvas would most likely be displayed on a separate layer and thus,
                    // that canvas's depth should be respected when propagating inputs.
                    break;
                }

                // Set node to parent
                curNode = curNode.parent;
            }
            while (curNode != null);
        }

        int IComparable<IInputReceiver>.CompareTo(IInputReceiver other)
        {
            // If not a ugui object, compare using input layer.
            if (!(other is UguiObject uguiOther))
                return other.InputLayer.CompareTo(InputLayer);

            int myIndex = inputPath.Count - 1;
            int otherIndex = uguiOther.inputPath.Count - 1;
            while (myIndex >= 0 && otherIndex >= 0)
            {
                int myDepth = inputPath[myIndex].Depth;
                int otherDepth = uguiOther.inputPath[otherIndex].Depth;
                if(myDepth != otherDepth)
                    return otherDepth.CompareTo(myDepth);

                myIndex--;
                otherIndex--;
            }
            // Prioritize whichever path with greater distance.
            return otherIndex.CompareTo(myIndex);
        }

        int IComparable<IGraphicObject>.CompareTo(IGraphicObject other)
        {
            return Depth.CompareTo(other.Depth);
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
        /// Unity object lifecycle method called on object enable.
        /// </summary>
        protected virtual void OnEnable()
        {
            // Support for OnEnable 
            if (isInited)
                OnEnableInited();
            isInited = true;
        }

        /// <summary>
        /// Additional method triggered during OnEnable.
        /// Is NOT called in the first invocation.
        /// </summary>
        protected virtual void OnEnableInited() {}

        /// <summary>
        /// Unity object lifecycle method called on object disable.
        /// </summary>
        protected virtual void OnDisable() {}

        /// <summary>
        /// Re-orders children based on their depth.
        /// </summary>
        private void ReorderChildren()
        {
            // Apply the order in hierarchy tree.
            for (int i = 0; i < children.Count; i++)
            {
                children[i].RawTransform.SetSiblingIndex(i);
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