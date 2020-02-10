using UnityEngine;
using UnityEngine.UI;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Graphics.Effects.Components
{
    public class MaskEffect : IEffect {

        /// <summary>
        /// The mask component affecting the object.
        /// </summary>
        public Mask Mask { get; private set; }

        public bool UsesMaterial => false;


        bool IEffect.Apply(IGraphicObject obj)
        {
            var graphic = GetGraphic(obj);
            if (graphic == null) return false;

            Mask = obj.RawObject.AddComponent<Mask>();
            return true;
        }

        void IEffect.Revert(IGraphicObject obj)
        {
            if(Mask == null) return;

            Object.Destroy(Mask);
            Mask = null;
        }

        /// <summary>
        /// Returns the Graphic component from the specified objct.
        /// </summary>
        protected Graphic GetGraphic(IGraphicObject obj)
        {
            var graphic = obj.RawObject.GetComponent<Graphic>();
            if (graphic == null)
            {
                Logger.LogWarning("MaskEffect.GetGraphic - Graphic component not found for object: " + obj.Name);
            }
            return graphic;
        }
    }
}