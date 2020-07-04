using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Networking.Linking;

namespace PBFramework.Platforms
{
    public class BaseHost : IPlatformHost {

        public virtual DeepLinker CreateDeepLinker() => new DeepLinker();
    }
}