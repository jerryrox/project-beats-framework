using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Platforms.OsxEditor.Networking.Linking;
using PBFramework.Networking.Linking;
using UnityEngine;

namespace PBFramework.Platforms.OsxEditor
{
    public class OsxEditorHost : BaseHost {

        public override DeepLinker CreateDeepLinker()
        {
            DeepLinker linker = base.CreateDeepLinker();
            DeepLinkReceiver receiver = new GameObject(nameof(DeepLinkReceiver)).AddComponent<DeepLinkReceiver>();
            receiver.Linker = linker;
            return linker;
        }
    }
}