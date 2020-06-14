using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Graphics.Effects.Shaders
{
    public abstract class BaseShaderEffect : IShaderEffect
    {
        private Material backupMaterial;


        public abstract Shader Shader { get; }

        public virtual bool UsesMaterial => true;

        /// <summary>
        /// Returns the shared material used for this particular shader.
        /// </summary>
        protected abstract Material Material { get; }


        bool IEffect.Apply(IGraphicObject obj)
        {
            var graphic = GetGraphic(obj);
            if (graphic == null) return false;

            graphic.material = Material;
            return true;
        }

        void IEffect.Revert(IGraphicObject obj)
        {
            var graphic = GetGraphic(obj);
            if (graphic == null) return;

            graphic.material = graphic.defaultMaterial;
        }

        /// <summary>
        /// Returns the Graphic component from the specified objct.
        /// </summary>
        protected Graphic GetGraphic(IGraphicObject obj)
        {
            var graphic = obj.RawObject.GetComponent<Graphic>();
            if (graphic == null)
            {
                Logger.LogWarning("BaseShaderEffect.GetGraphic - Graphic component not found for object: " + obj.Name);
            }
            return graphic;
        }
    }
}