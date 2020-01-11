using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public class HttpGetRequest : HttpRequest
    {
        public HttpGetRequest(string url, int timeout = 60, int retryCount = 1) : base(url, timeout, retryCount)
        {
        }

        protected override UnityWebRequest CreateWebRequester(string url)
            => UnityWebRequest.Get(url);
    }
}