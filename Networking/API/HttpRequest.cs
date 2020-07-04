using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using QueryParam = System.Collections.Generic.KeyValuePair<string, string>;

namespace PBFramework.Networking.API
{
    public abstract class HttpRequest : WebRequest, IHttpRequest {

        private Dictionary<string, string> headers = new Dictionary<string, string>();


        protected HttpRequest(string url, int timeout = 60, int retryCount = 1) : base("", timeout, retryCount)
        {
            this.link = new WebLink(url);
        }

        public void SetHeader(string key, string value)
        {
            if(string.IsNullOrEmpty(key)) throw new ArgumentException("Header key mustn't be null or empty space!");
            if(value == null) value = string.Empty;

            headers[key] = value;
        }

        public void SetCookies(string cookieString) => SetHeader("Cookie", cookieString);

        public void AddQueryParam(string key, string value) => link.SetParam(key, value);

        public void AddQueryParams(params QueryParam[] parameters) => AddQueryParams((IEnumerable<QueryParam>)parameters);

        public void AddQueryParams(IEnumerable<QueryParam> parameters)
        {
            foreach(var param in parameters)
                AddQueryParam(param.Key, param.Value);
        }

        protected override UnityWebRequest CreateRequest(string url)
        {
            var requester = CreateWebRequester(url);
            foreach (var header in headers)
                requester.SetRequestHeader(header.Key, header.Value);

            return requester;
        }

        /// <summary>
        /// Creates a new instance of the unity web requester object.
        /// </summary>
        protected abstract UnityWebRequest CreateWebRequester(string url);
    }
}