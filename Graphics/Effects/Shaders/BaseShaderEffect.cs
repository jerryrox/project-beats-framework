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
        public abstract Shader Shader { get; }

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
            if (graphic.material != Material) return;

            graphic.material = null;
        }

        /// <summary>
        /// Returns the Graphic component from the specified objct.
        /// </summary>
        protected Graphic GetGraphic(IGraphicObject obj)
        {
            var graphic = obj.RawObject.GetComponent<Graphic>();
            if (graphic == null)
            {
                Logger.LogWarning("AdditiveShaderEffect - Graphic component not found for object: " + obj.Name);
            }
            return graphic;
        }
    }
}