using UnityEngine;

namespace PBFramework.Graphics.Effects.Shaders
{
    public class AdditiveShaderEffect : BaseShaderEffect
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


        static AdditiveShaderEffect()
        {
            CachedShader = UnityEngine.Shader.Find("Effects/Additive");
            ShaderMaterial = new Material(CachedShader);
        }
    }
}