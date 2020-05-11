using System;
using System.Text;
using PBFramework.Debugging;

namespace PBFramework.Networking
{
    /// <summary>
    /// Custom implementation of Cookie since System.Net.CookieContainer imposes some limitations.
    /// </summary>
    public class Cookie {

        public string Name { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public bool HttpOnly { get; set; }

        public bool Secure { get; set; }

        /// <summary>
        /// Returns whether this cookie is valid for use.
        /// </summary>
        public bool IsValid => !IsForDeletion();


        /// <summary>
        /// Parses the specified cookie string and returns it.
        /// </summary>
        public static Cookie Parse(string value)
        {
            value = value.Trim();
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("value mustn't be null.");

            if(!value.EndsWith(";"))
                value = value + ';';

            Cookie cookie = new Cookie();
            StringBuilder sb = new StringBuilder();
            string currentKey = null;
            for (int i = 0; i < value.Length; i++)
            {
                // If key delimiter
                if (value[i] == '=')
                {
                    // If evaluating value but encountered this character, it's invalid.
                    if (currentKey != null)
                    {
                        throw new Exception($"Invalid character encountered while parsing value at index {i}");
                    }

                    currentKey = sb.ToString().Trim();
                    if (currentKey.Length == 0)
                        throw new Exception($"Invalid key parsed at index {i}");
                    sb.Length = 0;
                    continue;
                }
                // If value delimiter
                else if (value[i] == ';')
                {
                    string val = sb.ToString().Trim();
                    // If no key, assume this is a flag.
                    if (currentKey == null)
                    {
                        if(val.Equals("httponly", StringComparison.OrdinalIgnoreCase))
                            cookie.HttpOnly = true;
                        else if(val.Equals("secure", StringComparison.OrdinalIgnoreCase))
                            cookie.Secure = true;
                    }
                    else
                    {
                        if(currentKey.Equals("domain", StringComparison.OrdinalIgnoreCase))
                            cookie.Domain = val;
                        else if(currentKey.Equals("path", StringComparison.OrdinalIgnoreCase))
                            cookie.Path = val;
                        else if(currentKey.Equals("expires", StringComparison.OrdinalIgnoreCase))
                            cookie.Expires = DateTime.Parse(val);
                        else if (string.IsNullOrEmpty(cookie.Name))
                        {
                            cookie.Name = currentKey;
                            cookie.Value = val;
                        }
                    }
                    // Consume key
                    currentKey = null;
                    sb.Length = 0;
                    continue;
                }
                sb.Append(value[i]);
            }
            return cookie;
        }

        /// <summary>
        /// Tries parsing the specified cookie string.
        /// </summary>
        public static bool TryParse(string value, out Cookie cookie)
        {
            try
            {
                cookie = Parse(value);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Cookie.TryParse - Failed to parse value ({value}). Error: {e.Message}");

                cookie = null;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the expires value indicates cookie deletion.
        /// </summary>
        private bool IsForDeletion()
        {
            if(!Expires.HasValue)
                return false;
            var range = DateTime.Now - Expires.Value;
            return range.Days > 1;
        }
    }
}