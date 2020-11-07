#if UNITY_EDITOR
namespace PBFramework.Testing
{
    /// <summary>
    /// Types of events that triggers a TestAction's process.
    /// </summary>
    public enum ActionTrigger {
    
        Both = 0,
        Auto,
        Manual,
    }
}
#endif