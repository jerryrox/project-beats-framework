using System.IO;

namespace PBFramework.IO.Decoding
{
    /// <summary>
    /// An empty IDecoder for generalization purposes.
    /// </summary>
    public interface IDecoder
    {

    }

    /// <summary>
    /// Common interface used across different decoder types.
    /// </summary>
    public interface IDecoder<T> : IDecoder
        where T : new()
    {

        /// <summary>
        /// Instantiates a new instance of target type T.
        /// </summary>
        T CreateTarget();

        /// <summary>
        /// Decodes the specified stream and returns it.
        /// </summary>
        T Decode(StreamReader stream);

        /// <summary>
        /// Decodes the specified stream into the target.
        /// </summary>
        void Decode(StreamReader stream, T target);
    }
}