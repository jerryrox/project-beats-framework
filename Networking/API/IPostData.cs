using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public interface IPostData {

        /// <summary>
        /// Applies post data to the specified request.
        /// </summary>
        void ApplyData(UnityWebRequest request);
    }
}