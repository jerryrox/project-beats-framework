using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Services;
using PBFramework.Animations.Sections;

namespace PBFramework.Animations
{
    public class Anime : IAnime, IAnimeEditor {

        private float currentTime = 0f;
        private float duration = 0f;
        private float speed = 1f;

        private List<ISection> sections = new List<ISection>();


        public bool IsPlaying { get; private set; } = false;

        public float Time => currentTime;

        public float Duration => duration;

        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(value, 0.001f, float.MaxValue);
        }

        public WrapModes WrapMode { get; set; } = WrapModes.None;

        public StopModes StopMode { get; set; } = StopModes.Reset;



        public void AddEvent(float time, Action action)
        {
            if(time < 0f) throw new ArgumentException("time must be 0 or greater!");
            if(action == null) throw new ArgumentNullException(nameof(action));

            (this as IAnimeEditor).OnBuildSection(new EventSection(time, action));
        }

        public ISection<float> AnimateFloat(AnimateHandler<float> handler) => new Section<float>(this, handler);

        public ISection<int> AnimateInt(AnimateHandler<int> handler) => new Section<int>(this, handler);

        public ISection<Vector2> AnimateVector2(AnimateHandler<Vector2> handler) => new Section<Vector2>(this, handler);
        
        public ISection<Vector3> AnimateVector3(AnimateHandler<Vector3> handler) => new Section<Vector3>(this, handler);
        
        public ISection<Color> AnimateColor(AnimateHandler<Color> handler) => new Section<Color>(this, handler);

        public void Play()
        {
            if(IsPlaying) return;

            AnimeService.AddAnime(this);
            IsPlaying = true;
        }

        public void PlayFromStart()
        {
            Pause();
            Seek(0f);
            Play();
        }

        public void Pause()
        {
            AnimeService.RemoveAnime(this);
            IsPlaying = false;
        }

        public void Stop()
        {
            Pause();
            switch (StopMode)
            {
                case StopModes.None:
                    break;
                case StopModes.Reset:
                    Seek(0f);
                    break;
                case StopModes.End:
                    Seek(duration);
                    break;
            }
        }

        public void Seek(float time)
        {
            // Make sure the time is between 0 and duration.
            time = Mathf.Clamp(time, 0f, duration);

            // Store current time.
            currentTime = time;

            // Seek time for sections.
            SeekSections(time);
        }

        public bool Update(float deltaTime)
        {
            // Increase current time.
            currentTime += deltaTime * speed;

            // Update sections
            UpdateSections(currentTime);

            // If past the duration, wrap the animation.
            if (currentTime > duration)
            {
                switch (WrapMode)
                {
                    case WrapModes.None:
                        Pause();
                        return false;
                    case WrapModes.Reset:
                        Seek(0f);
                        Pause();
                        return false;
                    case WrapModes.Loop:
                        // Repeat seeking to beginning until below duration.
                        while (true)
                        {
                            // Decrement a loop worth of time from current.
                            currentTime -= duration;

                            // Seek back to beginning.
                            SeekSections(0f);
                            // If no longer need to loop, break out.
                            if(currentTime <= duration)
                                break;

                            // Perform update at the end time.
                            UpdateSections(duration);
                        }
                        // Perform update for current time.
                        UpdateSections(currentTime);
                        break;
                }
            }
            return true;
        }

        void IAnimeEditor.OnBuildSection(ISection section)
        {
            duration = Mathf.Max(section.Duration, duration);
            sections.Add(section);
        }

        /// <summary>
        /// Seeks all sections to specified time.
        /// </summary>
        private void SeekSections(float time)
        {
            for (int i = 0; i < sections.Count; i++)
                sections[i].SeekTime(time);
        }

        /// <summary>
        /// Updates all sections to specified time.
        /// </summary>
        private void UpdateSections(float time)
        {
            for (int i = 0; i < sections.Count; i++)
                sections[i].UpdateTime(time);
        }
    }
}