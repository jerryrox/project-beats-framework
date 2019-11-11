using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public class TextureRequest : AssetRequest {

        private bool nonReadable;

        public TextureRequest(string url, bool nonReadable = true) : base(url)
        {
            this.nonReadable = nonReadable;
        }

        protected override UnityWebRequest CreateRequest(string url)
        {
            return UnityWebRequestTexture.GetTexture(url, nonReadable);
        }
    }
}