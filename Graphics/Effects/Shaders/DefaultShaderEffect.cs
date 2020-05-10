using UnityEngine;
using UnityEngine.UI;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Graphics.Effects.Shaders
{
    /// <summary>
    /// A shader effect which simply utilizes the default shader with modifiable values.
    /// </summary>
    public class DefaultShaderEffect : IShaderEffect
    {
        private const string StencilCompName = "_StencilComp";
        private const string StencilOpName = "_StencilOp";


        private Material material;

        
        /// <summary>
        /// The Stencil Operation property of the shader.
        /// </summary>
        public StencilOp StencilOperation
        {
            get => (StencilOp)material.GetInt(StencilOpName);
            set => material.SetInt(StencilOpName, (int)value);
        }

        /// <summary>
        /// The Stencil Comparison property of the shader.
        /// </summary>
        public CompareFunction CompareFunction
        {
            get => (CompareFunction)material.GetInt(StencilCompName);
            set => material.SetInt(StencilCompName, (int)value);
        }

        public Shader Shader => material?.shader;

        public bool UsesMaterial => true;


        bool IEffect.Apply(IGraphicObject obj)
        {
            var graphic = GetGraphic(obj);
            if(graphic == null) return false;

            material = (graphic.material = Object.Instantiate(graphic.defaultMaterial));
            return material != null;
        }

        void IEffect.Revert(IGraphicObject obj)
        {
            var graphic = GetGraphic(obj);
            if(graphic == null) return;

            material = null;
            graphic.material = graphic.defaultMaterial;
        }

        /// <summary>
        /// Returns the graphic component on the object.
        /// </summary>
        protected Graphic GetGraphic(IGraphicObject obj)
        {
            var graphic = obj.RawObject.GetComponent<Graphic>();
            if (graphic == null)
            {
                Logger.LogWarning("StencilShaderEffect.GetGraphic - Graphic component not found for object: " + obj.Name);
            }
            return graphic;
        }
    }
}