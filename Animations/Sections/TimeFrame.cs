using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Utils;

namespace PBFramework.Animations.Sections
{
    public class TimeFrame<T> : IComparable<TimeFrame<T>> {
    
        /// <summary>
        /// The starting time of the time frame.
        /// </summary>
        public float Time;

        /// <summary>
        /// Function which returns the key value at current time.
        /// </summary>
        public Func<T> GetValue;

        /// <summary>
        /// Easing method for interpolation.
        /// </summary>
        public EaseType Ease;

        /// <summary>
        /// The cached multiplier which converts current playback time to an interpolant value between this and the next time frame.
        /// Will be assigned from Section during build.
        /// </summary>
        private float timeFactor;

        /// <summary>
        /// The duration of the time frame between this and the next time frame.
        /// </summary>
        private float duration;


        public TimeFrame(float time, Func<T> getValue, EaseType ease)
        {
            Time = time;
            GetValue = getValue;
            Ease = ease;
        }

        /// <summary>
        /// Returns whether the specified time is out of the time frame's range.
        /// </summary>
        public bool IsOverTime(float time)
        {
            return (time - Time) * timeFactor >= 1f;
        }

        /// <summary>
        /// Returns the interpolant value for specified time.
        /// </summary>
        public float GetInterpolant(float time)
        {
            return Easing.Ease((time - Time) * timeFactor, 0f, 1f, duration, Ease);
        }

        /// <summary>
        /// Links the time frame with the next frame.
        /// </summary>
        public void Link(TimeFrame<T> next)
        {
            duration = next.Time - Time;

            if(duration == 0f)
                timeFactor = 1f;
            else
                timeFactor = 1f / duration;
        }

        public int CompareTo(TimeFrame<T> other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}