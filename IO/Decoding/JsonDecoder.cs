using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBFramework.IO.Decoding
{
    public class JsonDecoder : Decoder<JObject> {

        private string header;


        static JsonDecoder()
        {
            Decoders.AddDecoder<JsonDecoder>("{", (header) => {
                var decoder = new JsonDecoder()
                {
                    header = header
                };
                return decoder;
            });
        }

        public override void Decode(StreamReader stream, JObject target)
        {
            var text = header + stream.ReadToEnd();
            var token = JsonConvert.DeserializeObject<JObject>(header);
            foreach (var pair in token)
            {
                target[pair.Key] = token[pair.Key];
            }
        }
    }
}