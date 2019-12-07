using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Graphics.UI.Elements;

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
            child.Inject(dependencies);
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
            base.SetParent(display);

            // Do additional process.
            if (display is NguiDisplay disp)
            {
                disp.parent = this;
            }
        }

        public override void SetParent(Transform transform)
        {
            if(transform is IDisplay display)
                SetParent(display);
            else
                base.SetParent(transform);
        }

        public override void SetParent(IObject obj)
        {
            if(obj is IDisplay display)
                SetParent(display);
            else
                base.SetParent(obj);
        }
    }
}