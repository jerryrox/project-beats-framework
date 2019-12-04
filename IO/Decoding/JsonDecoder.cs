using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBFramework.IO.Decoding
{
    public class JsonDecoder : Decoder<JObject> {

        private string header;


        public static void RegisterDecoder()
        {
            Decoders.AddDecoder<JObject>("{", (header) => {
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
            var token = JsonConvert.DeserializeObject<JObject>(text);
            foreach (var pair in token)
            {
                target[pair.Key] = token[pair.Key];
            }
        }
    }
}