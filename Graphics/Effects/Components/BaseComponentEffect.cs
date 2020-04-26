using UnityEngine;
using UnityEngine.UI;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Graphics.Effects.Components
{
    public abstract class BaseComponentEffect<T> : IEffect
        where T : Component
    {
        /// <summary>
        /// The component affecting the object.
        /// </summary>
        public T Component { get; private set; }

        public virtual bool UsesMaterial => false;


        bool IEffect.Apply(IGraphicObject obj) => OnApply(obj);

        void IEffect.Revert(IGraphicObject obj) => OnRevert();

        /// <summary>
        /// Event called when the effect needs to be applied to the host object.
        /// </summary>
        protected virtual bool OnApply(IGraphicObject obj)
        {
            if (GetGraphic(obj) == null)
                return false;

            Component = obj.RawObject.AddComponent<T>();
            return true;
        }

        /// <summary>
        /// Event called when the effect needs to be reverted from the effect.
        /// </summary>
        protected virtual void OnRevert()
        {
            if (Component == null)
                return;

            Object.Destroy(Component);
            Component = null;
        }

        /// <summary>
        /// Returns the Graphic component from the specified objct.
        /// </summary>
        protected Graphic GetGraphic(IGraphicObject obj)
        {
            var graphic = obj.RawObject.GetComponent<Graphic>();
            if (graphic == null)
            {
                Logger.LogWarning("BaseComponentEffect.GetGraphic - Graphic component not found for object: " + obj.Name);
            }
            return graphic;
        }
    }
}