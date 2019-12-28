using System;
using PBFramework.Utils;

namespace PBFramework.Animations.Sections
{
    public interface ISection
    {
        /// <summary>
        /// Returns the duration of the section.
        /// </summary>
        float Duration { get; }


        /// <summary>
        /// Finalizes the timeframe building procedure.
        /// </summary>
        void Build();

        /// <summary>
        /// Seeks for the time frame which contains the specified time.
        /// </summary>
        void SeekTime(float time);

        /// <summary>
        /// Updates the current time frame and if necessary, proceed to the next time frame.
        /// </summary>
        void UpdateTime(float time);
    }

    public interface ISection<T> : ISection
    {
        /// <summary>
        /// Adds a new timeframe at specified time, interpolating from current to target.
        /// </summary>
        ISection<T> AddTime(float time, T value, EaseType ease = EaseType.Linear);

        /// <summary>
        /// Adds a new timeframe at specified time, interpolating from current to volatile target.
        /// </summary>
        ISection<T> AddTime(float time, Func<T> value, EaseType ease = EaseType.Linear);
    }
}