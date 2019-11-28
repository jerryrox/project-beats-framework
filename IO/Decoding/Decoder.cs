using System.IO;

namespace PBFramework.IO.Decoding
{
    /// <summary>
    /// Provides the base implementation of the IDecoder interface.
    /// </summary>
    public abstract class Decoder<T> : IDecoder<T>
        where T : new()
    {

        public virtual T CreateTarget() => new T();

        public T Decode(StreamReader stream)
        {
            T target = CreateTarget();
            Decode(stream, target);
            return target;
        }

        public abstract void Decode(StreamReader stream, T target);
    }
}