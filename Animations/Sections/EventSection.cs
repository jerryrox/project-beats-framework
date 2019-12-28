using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Animations.Sections
{
    public class EventSection : ISection {

        private float lastTime;
        private Action action;


        public float Duration { get; private set; }


        public EventSection(float time, Action action)
        {
            Duration = time;
            this.action = action;
        }

        public void Build() {}

        public void SeekTime(float time)
        {
            lastTime = time;
        }

        public void UpdateTime(float time)
        {
            if (lastTime <= Duration && time > Duration)
            {
                action.Invoke();
            }
            lastTime = time;
        }
    }
}