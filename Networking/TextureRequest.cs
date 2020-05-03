using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public class TextureRequest : AssetRequest, IExplicitPromise<Texture2D> {

        public event Action<Texture2D> OnFinishedResult
        {
            add => OnFinished += () => value(response.TextureData);
            remove => OnFinished -= () => value(response.TextureData);
        }

        private bool nonReadable;


        public override object RawResult => response?.TextureData;
        Texture2D IPromise<Texture2D>.Result => response?.TextureData;


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