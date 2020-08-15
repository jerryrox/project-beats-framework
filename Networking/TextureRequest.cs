using UnityEngine;
using UnityEngine.Networking;
using PBFramework.Data.Bindables;
using PBFramework.Threading.Futures;

namespace PBFramework.Networking
{
    public class TextureRequest : AssetRequest, IControlledFuture<Texture2D> {

        private Bindable<Texture2D> output = new Bindable<Texture2D>(null)
        {
            TriggerWhenDifferent = true
        };

        private bool nonReadable;


        public IReadOnlyBindable<Texture2D> Output => output;

        public override object RawResult => response?.TextureData;


        public TextureRequest(string url, bool nonReadable = true) : base(url)
        {
            this.nonReadable = nonReadable;
        }

        protected override void DisposeSoft()
        {
            base.DisposeSoft();
            if (!IsDisposed.Value)
                output.Value = null;
        }

        protected override void EvaluateResponse()
        {
            output.Value = response.TextureData;
        }

        protected override UnityWebRequest CreateRequest(string url)
        {
            return UnityWebRequestTexture.GetTexture(url, nonReadable);
        }
    }
}