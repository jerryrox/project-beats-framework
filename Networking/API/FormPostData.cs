using UnityEngine;
using UnityEngine.Networking;

namespace PBFramework.Networking.API
{
    public class FormPostData : IPostData {

        private WWWForm form;


        public FormPostData(WWWForm form = null)
        {
            this.form = form ?? new WWWForm();
        }

        public void AddField(string key, string value) => form.AddField(key, value);

        public void AddField(string key, int value) => form.AddField(key, value);

        public void AddBinary(string key, byte[] data) => form.AddBinaryData(key, data);

        public void AddFile(string key, byte[] data, string fileName) => form.AddBinaryData(key, data, fileName);

        public void AddFile(string key, byte[] data, string fileName, string mimeType) => form.AddBinaryData(key, data, fileName, mimeType);

        public void ApplyData(UnityWebRequest request)
        {
            request.uploadHandler = new UploadHandlerRaw(form.data);
            foreach (var header in form.headers)
            {
                Debug.Log($"Setting header: ({header.Key}, {header.Value})");
                request.SetRequestHeader(header.Key, header.Value);
            }
        }
    }
}