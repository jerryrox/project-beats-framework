using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public class HttpPostRequest : HttpRequest {

        private IPostData postData;


        public HttpPostRequest(string url, int timeout = 60, int retryCount = 1) : base(url, timeout, retryCount)
        {
        }

        /// <summary>
        /// Sets the data for the post request.
        /// </summary>
        public void SetPostParam(IPostData postData) => this.postData = postData;

        protected override UnityWebRequest CreateWebRequester(string url)
        {
            var request = UnityWebRequest.Post(url, "");
            if(postData != null)
                postData.ApplyData(request);
            return request;
        }
    }
}