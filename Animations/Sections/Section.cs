using System;
using System.Collections;
using PBFramework.Data;
using PBFramework.Utils;
using PBFramework.Exceptions;
using PBFramework.Animations.Interpolations;

namespace PBFramework.Animations.Sections
{
    public class Section<T> : ISection<T> {

        private IAnimeEditor anime;
        private bool isBuilt = false;

        private IInterpolator<T> interpolator;
        private AnimateHandler<T> handler;
        private SortedList<TimeFrame<T>> timeFrames = new SortedList<TimeFrame<T>>();

        /// <summary>
        /// Current time frame index being updated for.
        /// </summary>
        private int frameIndex;


        public float Duration { get; private set; }


        public Section(IAnimeEditor anime, AnimateHandler<T> handler)
        {
            this.anime = anime;
            this.handler = handler;
            interpolator = InterpolatorHelper.GetInterpolator<T>();
        }

        public ISection<T> AddTime(float time, T value, EaseType ease = EaseType.Linear)
        {
            return AddTime(time, () => value, ease);
        }

        public ISection<T> AddTime(float time, Func<T> value, EaseType ease = EaseType.Linear)
        {
            if(isBuilt) throw new ImmutableException("This section has already been built!");
            if(time < 0f) throw new ArgumentException("time must be 0 or greater!");
            if(value == null) throw new ArgumentNullException(nameof(value));

            timeFrames.Add(new TimeFrame<T>(time, value, ease));
            return this;
        }

        public void Build()
        {
            if(isBuilt) throw new ImmutableException("This section has already been built!");
            if(timeFrames.Count == 0) throw new Exception("There must be at least one time frame in a section!");

            // If the section doesn't start from time 0, fill it in.
            if (timeFrames[0].Time != 0f)
            {
                var firstFrame = timeFrames[0];
                timeFrames.Add(new TimeFrame<T>(
                    0f,
                    firstFrame.GetValue,
                    EaseType.Linear
                ));
            }

            // Link the time frames.
            for (int i = 0; i < timeFrames.Count-1; i++)
                timeFrames[i].Link(timeFrames[i+1]);

            // Determine the duration.
            Duration = timeFrames[timeFrames.Count - 1].Time;

            // Notify container anime
            anime.OnBuildSection(this);

            // This section is now built.
            isBuilt = true;
        }

        public void SeekTime(float time)
        {
            // Reset frame index.
            frameIndex = timeFrames.Count-1;

            // Determine which frame index should the specified time belong to.
            for (int i = 0; i < timeFrames.Count; i++)
            {
                var frame = timeFrames[i];

                if (time <= frame.Time)
                {
                    frameIndex = i;
                    break;
                }
            }

            if (this is EventSection)
            {
                
            }

            // Update state for new time frame.
            UpdateTime(time);
        }

        public void UpdateTime(float time)
        {
            if (frameIndex >= timeFrames.Count)
                return;

            for (int i = frameIndex; i < timeFrames.Count; i++)
            {
                var frame = timeFrames[i];

                // Update only if interpolant is less than 1.
                // Otherwise, frame index must be the last one.
                if (frame.IsOverTime(time) && i < timeFrames.Count - 1)
                {
                    // Advance to the next frame.
                    frameIndex++;
                }
                else
                {
                    if (i == timeFrames.Count - 1)
                    {
                        // Set frame index to the end to prevent further updates.
                        frameIndex = timeFrames.Count;

                        // Just invoke with the last key value.
                        handler.Invoke(frame.GetValue.Invoke());
                    }
                    else
                    {
                        // Calculate interpolant
                        float interpolant = frame.GetInterpolant(time);

                        // Notify the handler.
                        var nextFrame = timeFrames[i + 1];
                        handler.Invoke(interpolator.Interpolate(frame.GetValue.Invoke(), nextFrame.GetValue.Invoke(), interpolant));
                    }
                    break;
                }
            }
        }
    }
}