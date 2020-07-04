using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Networking.Linking;

namespace PBFramework.Platforms.OsxEditor.Networking.Linking
{
    /// <summary>
    /// An component which pipes deep link urls to the given DeepLinker instance.
    /// </summary>
    public class DeepLinkReceiver : MonoBehaviour {

        [SerializeField]
        private string url;


        /// <summary>
        /// The url to be used when triggering deep link emission.
        /// </summary>
        public string Url
        {
            get => url;
            set => url = value;
        }

        /// <summary>
        /// The linker instance to use for linking.
        /// </summary>
        public DeepLinker Linker { get; set; }


        [ContextMenu("Trigger link")]
        public void TriggerLink()
        {
            if(Linker == null)
                return;
            Linker.Emit(url);
        }
    }
}