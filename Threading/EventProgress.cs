using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading
{
    public class EventProgress : SimpleProgress, IEventProgress {

        public event Action OnFinished;


        public void InvokeFinished() => OnFinished?.Invoke();
    }
}