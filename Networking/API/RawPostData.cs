using System.Text;
using PBFramework.Debugging;
using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public class RawPostData : IPostData {

        private string text;
        private Encoding encoding;
        private Types type;


        public RawPostData(string text, Types type, Encoding encoding = null)
        {
            this.text = text ?? string.Empty;
            this.type = type;
            this.encoding = encoding ?? Encoding.UTF8;
        }

        public void ApplyData(UnityWebRequest request)
        {
            request.uploadHandler = new UploadHandlerRaw(encoding.GetBytes(text));
            request.SetRequestHeader("Content-Type", GetContentType());
        }

        private string GetContentType()
        {
            switch (type)
            {
                case Types.Text: return "text/plain";
                case Types.Json: return "application/json";
                case Types.Javascript: return "application/javascript";
                case Types.Html: return "text/html";
                case Types.Xml: return "application/xml";
            }
            Logger.LogWarning($"RawPostData.GetContentType - Unknown type: {type}");
            return "";
        }


        /// <summary>
        /// Types of raw data that can be assigned.
        /// </summary>
        public enum Types
        {
            Text,
            Json,
            Javascript,
            Html,
            Xml
        }
    }
}