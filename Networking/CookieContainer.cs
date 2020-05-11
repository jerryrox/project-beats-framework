using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Networking
{
    /// <summary>
    /// Custom implementation of CookieContainer since System.Net.CookieContainer imposes limitations.
    /// </summary>
    public class CookieContainer {

        private Dictionary<string, Cookie> cookies = new Dictionary<string, Cookie>();


        /// <summary>
        /// Returns the cookie with specified name.
        /// </summary>
        public Cookie this[string name] => cookies[name];


        /// <summary>
        /// Returns whether the cookie of specified name exists.
        /// </summary>
        public bool HasName(string name) => cookies.ContainsKey(name);

        /// <summary>
        /// Returns the stringified cookies for sending a request.
        /// </summary>
        public string GetCookieString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cookie in cookies.Values)
            {
                if(sb.Length > 0)
                    sb.Append("; ");
                sb.Append($"{cookie.Name}={cookie.Value}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Sets headers based on the specified Set-Cookie value.
        /// </summary>
        public void SetCookie(string header)
        {
            foreach (var entry in ParseEntries(header))
            {
                if (Cookie.TryParse(entry, out Cookie cookie) && cookie.IsValid)
                    AddCookie(cookie);
            }
        }

        /// <summary>
        /// Adds the specified cookie to the container, replacing the existing cookie of equivalent name.
        /// </summary>
        public void AddCookie(Cookie cookie)
        {
            if(string.IsNullOrEmpty(cookie.Name))
                return;
            cookies[cookie.Name] = cookie;
        }

        /// <summary>
        /// Returns all cookie entries from specified header value.
        /// </summary>
        private IEnumerable<string> ParseEntries(string header)
        {
            Func<int, bool> isSeparationComma = (commaIndex) =>
            {
                // Try to encounter a '=' or ';' character.
                for (int i = commaIndex+1; i < header.Length; i++)
                {
                    switch (header[i])
                    {
                        case '=': return true;
                        case ';': return false;
                    }
                }
                return false;
            };

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < header.Length; i++)
            {
                // If comma at the end of the header, or a non-white space char after it.
                if (header[i] == ',' && isSeparationComma(i))
                {
                    // Return progress
                    yield return sb.ToString();
                    sb.Length = 0;
                    continue;
                }
                else
                {
                    sb.Append(header[i]);
                }
            }

            // Flush
            if(sb.Length > 0)
                yield return sb.ToString();
        }
    }
}