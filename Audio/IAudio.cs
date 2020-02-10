using System;

namespace PBFramework.Audio
{
    /// <summary>
    /// Representation of an audio clip.
    /// </summary>
    public interface IAudio : IDisposable {
        
        /// <summary>
        /// Returns the duration of the audio in milliseconds.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// Returns the frequency of the audio.
        /// </summary>
        int Frequency { get; }

        /// <summary>
        /// Returns the number of channels of the audio.
        /// </summary>
        int Channels { get; }
    }
}