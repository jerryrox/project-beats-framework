using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBFramework.UI.UGUI
{
    public class CustomScrollRect : ScrollRect {

        /// <summary>
        /// Event called on scroll rect drag started by input.
        /// </summary>
        public event Action OnDragStart;


        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            OnDragStart?.Invoke();
        }
    }
}