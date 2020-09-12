using UnityEngine;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public class TextureRequest : AssetRequest<Texture2D> {

        private bool nonReadable;


        public TextureRequest(string url, bool nonReadable = true) : base(url)
        {
            this.nonReadable = nonReadable;
        }

        protected override void EvaluateResponse()
        {
            Output = response.TextureData;
        }

        protected override UnityWebRequest CreateRequest(string url)
        {
            return UnityWebRequestTexture.GetTexture(url, nonReadable);
        }
    }
}