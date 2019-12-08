using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Graphics.UI.Elements.NGUI;

namespace PBFramework.Graphics.UI
{
    /// <summary>
    /// IDisplay implementation using NGUI.
    /// </summary>
    public class NguiDisplay : GObject, IDisplay
    {
        private IDisplay parent;


        public IDisplay Parent => parent;


        protected override void Awake()
        {
            base.Awake();
        }

        public new IDisplay CreateChild()
        {
            var child = new GameObject().AddComponent<NguiDisplay>();
            child.SetParent(this);
            dependencies?.Inject(child);
            return child;
        }

        public void AddChild(IDisplay display) => base.AddChild(display);

        public void AddChildren(IEnumerable<IDisplay> displays)
        {
            foreach(var child in displays)
                AddChild(child);
        }

        public void SetParent(IDisplay display)
        {
            display.Layer = Layer;
            base.SetParent(display);

            // Do additional process.
            if (display is NguiDisplay disp)
            {
                disp.parent = this;
            }
        }

        public override void SetParent(Transform transform)
        {
            transform.gameObject.layer = myObject.layer;
            if(transform is IDisplay display)
                SetParent(display);
            else
                base.SetParent(transform);
        }

        public override void SetParent(IObject obj)
        {
            obj.Layer = Layer;
            if(obj is IDisplay display)
                SetParent(display);
            else
                base.SetParent(obj);
        }

        public override T AddComponent<T>()
        {
            var component = myObject.AddComponent<T>();
            if(component is NguiElement element)
            {
                element.Display = this;
            }
            // Dependencies must be injected after element's display has been set, if applicable.
            dependencies?.Inject(component);
            return component;
        }
    }
}