using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public class WebResponse : IWebResponse {

        private WebRequest parent;
        private UnityWebRequest request;


        public long Code => request.responseCode == 0 ? request.GetResponseHeaders().GetResponseCode() : request.responseCode;

        public bool IsSuccess 
        {
            get => parent.IsDone && !request.isNetworkError &&
                !request.isHttpError && string.IsNullOrEmpty(request.error);
        }

        public string ErrorMessage => request.error;

        public string TextData => request.downloadHandler.text;

        public byte[] ByteData => request.downloadHandler.data;

        public AudioClip AudioData => ((DownloadHandlerAudioClip)request.downloadHandler).audioClip;

        public AssetBundle AssetBundleData => DownloadHandlerAssetBundle.GetContent(request);

        public Texture2D TextureData => DownloadHandlerTexture.GetContent(request);

        public Dictionary<string, string> Headers => request.GetResponseHeaders();

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

        public ulong BytesLoaded => request.downloadedBytes;


        public WebResponse(WebRequest parent, UnityWebRequest request)
        {
            this.parent = parent;
            this.request = request;
        }

        public void Dispose()
        {
            request = null;
        }
    }
}