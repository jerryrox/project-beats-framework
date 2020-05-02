namespace PBFramework.Graphics.Effects.Shaders
{
    /// <summary>
    /// Types of stencil operations for the default UGUI shader.
    /// https://github.com/supyrb/ConfigurableShaders/wiki/Stencil
    /// </summary>
    public enum StencilOp
    {
        Keep = 0,
        Zero = 1,
        Replace = 2,
        IncrementSaturate = 3,
        DecrementSaturate = 4,
        Invert = 5,
        IncrementWrap = 6,
        DecrementWrap = 7
    }
}