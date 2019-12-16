using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI
{
    public class UguiToggle : UguiObject<Toggle>, IToggle {

        public event Action<bool> OnChange;

        protected CanvasGroup canvasGroup;

        protected UguiSprite background;
        protected UguiSprite tick;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public bool UseFade
        {
            get => component.toggleTransition == Toggle.ToggleTransition.Fade;
            set => component.toggleTransition = value ? Toggle.ToggleTransition.Fade : Toggle.ToggleTransition.None;
        }

        public bool Value
        {
            get => component.isOn;
            set => component.isOn = value;
        }

        public ISprite Background => background;

        public ISprite Tick => tick;


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();
            background = AddComponentInject<UguiSprite>();
            tick = CreateChild<UguiSprite>("tick", 1);

            background.ImageType = Image.Type.Sliced;

            component.onValueChanged.AddListener((value) => OnChange?.Invoke(value));
            component.targetGraphic = background.GetComponent<Image>();
            component.graphic = tick.GetComponent<Image>();

            Value = false;
            Size = new Vector2(36f, 36f);
        }
    }
}