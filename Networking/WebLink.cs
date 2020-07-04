using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    /// <summary>
    /// Object which parses and builds web urls.
    /// </summary>
    public class WebLink {

        private string scheme = "";
        private string path = "";
        private Dictionary<string, string> parameters = new Dictionary<string, string>();


        /// <summary>
        /// Returns the scheme of the url.
        /// </summary>
        public string Scheme => scheme;

        /// <summary>
        /// Returns the path of the url.
        /// </summary>
        public string Path => path;

        /// <summary>
        /// Returns the parameters of the url.
        /// </summary>
        public IReadOnlyDictionary<string, string> Parameters => parameters;

        /// <summary>
        /// Returns the full url generated from current state.
        /// </summary>
        public string Url
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(scheme).Append("://");
                if(scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
                    sb.Append('/');
                sb.Append(path);
                
                if (parameters.Count > 0)
                {
                    sb.Append('?');
                    
                    bool isFirst = true;
                    foreach (var pair in parameters)
                    {
                        if(!isFirst)
                            sb.Append('&');
                        sb.Append(UnityWebRequest.EscapeURL(pair.Key));
                        sb.Append('=');
                        sb.Append(UnityWebRequest.EscapeURL(pair.Value));
                        isFirst = false;
                    }
                }
                return sb.ToString();
            }
        }


        public WebLink(string path = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                this.scheme = ExtractScheme(path);
                this.path = ExtractPath(path);
                ParseParams(ExtractParams(path));
            }
        }

        /// <summary>
        /// Extracts only the scheme value from the specified raw path value.
        /// </summary>
        public static string ExtractScheme(string path)
        {
            int schemeIndex = path.IndexOf("://");
            if(schemeIndex >= 0)
                return path.Substring(0, schemeIndex).Trim();
            return "";
        }

        /// <summary>
        /// Extracts only the path value from the specified raw path value.
        /// </summary>
        public static string ExtractPath(string path)
        {
            int schemeInx = path.IndexOf("://");
            if(schemeInx >= 0)
                path = path.Substring(schemeInx + 3);
            int queryInx = path.IndexOf('?');
            if(queryInx >= 0)
                path = path.Substring(0, queryInx);
            return path.Trim('/', ' ');
        }

        /// <summary>
        /// Extracts the query parameter string from the specified path.
        /// </summary>
        public static string ExtractParams(string path)
        {
            if(string.IsNullOrEmpty(path))
                return "";
            int queryInx = path.IndexOf('?');
            if(queryInx >= 0)
                return path.Substring(queryInx + 1);
            return "";
        }

        /// <summary>
        /// Sets the scheme of the url.
        /// </summary>
        public void SetScheme(string scheme)
        {
            string newScheme = ExtractScheme(scheme);
            if (string.IsNullOrEmpty(newScheme))
                newScheme = scheme.Trim();
            this.scheme = newScheme;
        }

        /// <summary>
        /// Sets the path of the url.
        /// </summary>
        public void SetPath(string path)
        {
            this.path = ExtractPath(path);
        }

        /// <summary>
        /// Sets the specified param key/value pair.
        /// </summary>
        public void SetParam(string key, string value)
        {
            if(string.IsNullOrEmpty(key))
                return;
            this.parameters[key] = value;
        }

        /// <summary>
        /// Parses the specified raw param string into the Params table.
        /// </summary>
        private void ParseParams(string param)
        {
            string curKey = null;
            StringBuilder sb = new StringBuilder();
            bool parsingKey = true;

            Action breakEquals = () =>
            {
                parsingKey = false;
                curKey = UnityWebRequest.UnEscapeURL(sb.ToString().Trim());
                sb.Length = 0;
            };
            Action breakAmpersand = () =>
            {
                parsingKey = true;
                if (!string.IsNullOrEmpty(curKey))
                    parameters[curKey] = UnityWebRequest.UnEscapeURL(sb.ToString().Trim());
                curKey = null;
                sb.Length = 0;
            };

            for (int i = 0; i < param.Length; i++)
            {
                char curChar = param[i];
                switch (curChar)
                {
                    case '=':
                        breakEquals.Invoke();
                        break;
                    case '&':
                        breakAmpersand.Invoke();
                        break;
                    default:
                        sb.Append(curChar);
                        break;
                }
            }

            // Handle last value parsing if applicable.
            if (!parsingKey)
                breakAmpersand.Invoke();
        }
    }
}