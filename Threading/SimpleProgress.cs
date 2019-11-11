using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading
{
    /// <summary>
    /// A basic implementation of IProgress interface.
    /// </summary>
    public class SimpleProgress : ISimpleProgress {

        public float Progress { get; set; }


        public void Report(float progress) => Progress = progress;
    }
}