using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public class UguiPanel : UguiObject, IPanel
    {
        protected CanvasGroup group;
        protected Mask mask;
        protected Image image;


        public float Alpha
        {
            get => group.alpha;
            set => group.alpha = value;
        }

        public bool UseMask
        {
            get => mask.enabled;
            set => mask.enabled = image.enabled = value;
        }

        public bool ShowMaskingSprite
        {
            get => mask.showMaskGraphic;
            set => mask.showMaskGraphic = value;
        }

        public Sprite MaskSprite
        {
            get => image.sprite;
            set => image.sprite = value;
        }


        protected override void Awake()
        {
            base.Awake();
            group = myObject.AddComponent<CanvasGroup>();
            mask = myObject.AddComponent<Mask>();
            image = myObject.AddComponent<Image>();

            UseMask = false;
            ShowMaskingSprite = false;
        }
    }
}