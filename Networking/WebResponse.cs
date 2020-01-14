using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public class WebResponse : IWebResponse {

        private WebRequest parent;


        public UnityWebRequest Request { get; set; }

        public long Code => Request == null ? 0 : (Request.responseCode == 0 ? Request.GetResponseHeaders().GetResponseCode() : Request.responseCode);

        public bool IsSuccess 
        {
            get => Request != null && parent.IsDone && !Request.isNetworkError &&
                !Request.isHttpError && string.IsNullOrEmpty(Request.error);
        }

        public string ErrorMessage => Request?.error;

        public string TextData => Request?.downloadHandler.text;

        public byte[] ByteData => Request?.downloadHandler.data;

        public AudioClip AudioData => Request == null ? null : ((DownloadHandlerAudioClip)Request.downloadHandler).audioClip;

        public AssetBundle AssetBundleData => Request == null ? null : DownloadHandlerAssetBundle.GetContent(Request);

        public Texture2D TextureData => Request == null ? null : DownloadHandlerTexture.GetContent(Request);

        public Dictionary<string, string> Headers => Request?.GetResponseHeaders();

        public string ContentType
        {
            get
            {
				var headers = Headers;
				if(headers == null)
					return null;
                var type = headers.FirstOrDefault(p => p.Key.Equals("content-type", StringComparison.OrdinalIgnoreCase));
				return type.Value;
            }
        }

        public long ContentLength
        {
            get
            {
				var headers = Headers;
				if(headers == null)
					return 0;
                var type = headers.FirstOrDefault(p => p.Key.Equals("content-length", StringComparison.OrdinalIgnoreCase));
                if(long.TryParse(type.Value, out long result))
                    return result;
                return 0;
            }
        }

        public ulong BytesLoaded => Request == null ? 0 : Request.downloadedBytes;


        public WebResponse(WebRequest parent)
        {
            this.parent = parent;
        }

        public void Dispose()
        {
            Request = null;
        }
    }
}