using UnityEngine;

namespace PBFramework.Graphics.Effects.Shaders
{
    public interface IShaderEffect : IEffect {
    
        /// <summary>
        /// Returns the shader used by the effect.
        /// </summary>
        Shader Shader { get; }
    }
}