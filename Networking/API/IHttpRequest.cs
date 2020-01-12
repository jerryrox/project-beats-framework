using System.Collections.Generic;

using QueryParam = System.Collections.Generic.KeyValuePair<string, string>;

namespace PBFramework.Networking.API
{
    /// <summary>
    /// Extension of web request specialized for API calls.
    /// </summary>
    public interface IHttpRequest : IWebRequest {
    
        /// <summary>
        /// Returns the endpoint of the request.
        /// </summary>
        string Endpoint { get; }

        /// <summary>
        /// Returns the query param string of the request url.
        /// </summary>
        string QueryString { get; }


        /// <summary>
        /// Sets the specified header pair to the request.
        /// </summary>
        void SetHeader(string key, string value);

        /// <summary>
        /// Sets the specified cookies to the request.
        /// </summary>
        void SetCookies(string cookieString);

        /// <summary>
        /// Adds a query parameter of specified key and value.
        /// </summary>
        void AddQueryParam(string key, string value);

        /// <summary>
        /// Adds multiple query parameters at once within the specified array.
        /// </summary>
        void AddQueryParams(params QueryParam[] parameters);

        /// <summary>
        /// Adds multiple query parameters at once within the specified range.
        /// </summary>
        void AddQueryParams(IEnumerable<QueryParam> parameters);
    }
}