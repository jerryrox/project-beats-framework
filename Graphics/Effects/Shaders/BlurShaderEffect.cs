using UnityEngine;

namespace PBFramework.Graphics.Effects.Shaders
{
    public class BlurShaderEffect : BaseShaderEffect
    {
        /// <summary>
        /// Shader cached for performance.
        /// </summary>
        private static Shader CachedShader;

        /// <summary>
        /// The material shared across the same effect for performance.
        /// </summary>
        private static Material ShaderMaterial;


        public override Shader Shader => CachedShader;

        protected override Material Material => ShaderMaterial;


        static BlurShaderEffect()
        {
            CachedShader = UnityEngine.Shader.Find("Effects/Blur");
            ShaderMaterial = new Material(CachedShader);
            ShaderMaterial.SetInt("_Radius", 3);
        }
    }
}