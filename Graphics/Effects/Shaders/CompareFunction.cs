namespace PBFramework.Graphics.Effects.Shaders
{
    /// <summary>
    /// Types of comparision functions to compare between the reference value and value in buffer.
    /// https://github.com/supyrb/ConfigurableShaders/wiki/Stencil
    /// </summary>
    public enum CompareFunction
    {
        Disabled = 0,
        Never = 1,
        Less = 2,
        Equal = 3,
        LessEqual = 4,
        Greater = 5,
        NotEqual = 6,
        GreaterEqual = 7,
        Always = 8
    }
}