using PBFramework.Networking.Linking;

namespace PBFramework.Platforms
{
    /// <summary>
    /// Interface of a platform-specific functionality provider based on different hosts.
    /// </summary>
    public interface IPlatformHost {

        /// <summary>
        /// Creates a new deeplinker instance.
        /// </summary>
        DeepLinker CreateDeepLinker();
    }
}