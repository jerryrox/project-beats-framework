using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public class BinaryPostData : IPostData {

        private byte[] data;


        public BinaryPostData(byte[] data)
        {
            this.data = data;
        }

        public void ApplyData(UnityWebRequest request)
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.SetRequestHeader("Content-Type", "application/octet-stream");
        }
    }
}