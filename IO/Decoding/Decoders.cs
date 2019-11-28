using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.IO.Decoding
{
    public class Decoders {

        private static Decoders I;

		/// <summary>
		/// Table of decoder creators.
		/// </summary>
		private Dictionary<Type, Dictionary<string, DecoderCreateHandler>> decoders;


        private static Decoders Instance => I ?? (I = new Decoders());


        /// <summary>
        /// Delegate for decoder instance creation handler.
        /// </summary>
        public delegate IDecoder DecoderCreateHandler(string header);


        public Decoders()
        {
            decoders = new Dictionary<Type, Dictionary<string, DecoderCreateHandler>>();

        }

        /// <summary>
        /// Returns a new decoder instance for specified stream.
        /// </summary>
        public static IDecoder<T> GetDecoder<T>(StreamReader stream) where T : new()
        {
            if(stream == null) throw new ArgumentNullException(nameof(stream));

            // Check whether a decoder for specified type has been registered.
            if (Instance.decoders.TryGetValue(typeof(T), out Dictionary<string, DecoderCreateHandler> table))
            {
                // Read the first line.
                string firstLine = null;
                while (string.IsNullOrEmpty(firstLine) && !stream.EndOfStream)
                {
                    firstLine = stream.ReadLine();
                    if(firstLine != null)
                        firstLine.Trim();
                }

                // If not determinable, return.
                if (string.IsNullOrEmpty(firstLine))
                    throw new IOException($"Failed to read the first line of the stream");
                
                // Select the decoder that matches the first line identification.
                var decoder = table.Select(d => firstLine.StartsWith(d.Key) ? d.Value : null).FirstOrDefault();
                if (decoder == null)
                {
                    Logger.LogWarning($"Decoders.GetDecoder - No decoder registered that matches the given first line ({firstLine})");
                    return null;
                }

                // Return decoder.
                return (IDecoder<T>)decoder.Invoke(firstLine);
            }

            // No matching type.
            Logger.LogWarning($"Decoders.GetDecoder - No decoder table found for type: {typeof(T).Name}");
            return null;
        }

        /// <summary>
        /// Registers a new decoder creation handler for specified header.
        /// </summary>
        public static void AddDecoder<T>(string header, DecoderCreateHandler handler)
        {
            if(!Instance.decoders.TryGetValue(typeof(T), out Dictionary<string, DecoderCreateHandler> table))
				Instance.decoders.Add(typeof(T), table = new Dictionary<string, DecoderCreateHandler>());
			table[header] = handler;
        }
    }
}