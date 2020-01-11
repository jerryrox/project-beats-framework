using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using QueryParam = System.Collections.Generic.KeyValuePair<string, string>;

namespace PBFramework.Networking.API
{
    public abstract class HttpRequest : WebRequest, IHttpRequest {

        private Dictionary<string, string> queryParams = new Dictionary<string, string>();

        private Dictionary<string, string> headers = new Dictionary<string, string>();


        public string Endpoint { get; protected set; }

        public string QueryString
        {
            get
            {
                if(queryParams.Count == 0) return string.Empty;

                StringBuilder sb = new StringBuilder();
                foreach (var param in queryParams)
                {
                    if(sb.Length > 0)
                        sb.Append('&');
                    sb.Append(UnityWebRequest.EscapeURL(param.Key)).Append('=').Append(UnityWebRequest.EscapeURL(param.Value));
                }
                return sb.ToString();
            }
        }

        public override string Url => $"{Endpoint}{(queryParams.Count == 0 ? string.Empty : "?")}{QueryString}";


        protected HttpRequest(string url, int timeout = 60, int retryCount = 1) : base("", timeout, retryCount)
        {
            url = url.Trim();
            
            ExtractEndpoint(url);
            ExtractQueryParams(url);
        }

        public void SetHeader(string key, string value)
        {
            if(string.IsNullOrEmpty(key)) throw new ArgumentException("Header key mustn't be null or empty space!");
            if(value == null) value = string.Empty;

            headers[key] = value;
        }

        public void AddQueryParam(string key, string value)
        {
            if(key == null) key = string.Empty;
            if(value == null) value = string.Empty;

            queryParams[key] = value;
        }

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

        /// <summary>
        /// Removes query parameters and extracts the endpoint from specified url.
        /// </summary>
        private void ExtractEndpoint(string url)
        {
            int delimiter = url.IndexOf('?');
            if (delimiter < 0)
            {
                Endpoint = url;
                return;
            }

            Endpoint = url.Substring(0, delimiter);
        }

        /// <summary>
        /// Removes endpoint and extracts the query parameters from specified url.
        /// </summary>
        private void ExtractQueryParams(string url)
        {
            int delimiter = url.IndexOf('?');
            if(delimiter < 0 || delimiter == url.Length - 1) return;

            string rawParams = url.Substring(delimiter+1);
            foreach (var parameter in rawParams.Split('&'))
            {
                var pair = parameter.Split('=');
                if(pair.Length != 2) continue;

                AddQueryParam(
                    UnityWebRequest.UnEscapeURL(pair[0].Trim()),
                    UnityWebRequest.UnEscapeURL(pair[1].Trim())
                );
            }
        }
    }
}