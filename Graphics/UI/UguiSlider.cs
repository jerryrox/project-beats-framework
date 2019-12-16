using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI
{
    public class UguiSlider : UguiObject<Slider>, ISlider {

        public event Action<float> OnChange;

        protected UguiSprite background;
        protected UguiObject foregroundArea;
        protected UguiSprite foreground;
        protected UguiObject thumbArea;
        protected UguiSprite thumb;


        public ISprite Background => background;

        public ISprite Foreground => foreground;

        public ISprite Thumb => thumb;

        public Slider.Direction Direction
        {
            get => component.direction;
            set => component.direction = value;
        }

        public bool IsWholeNumber
        {
            get => component.wholeNumbers;
            set => component.wholeNumbers = value;
        }

        public float MinValue
        {
            get => component.minValue;
            set => component.minValue = value;
        }

        public float MaxValue
        {
            get => component.maxValue;
            set => component.maxValue = value;
        }

        public float Value
        {
            get => component.value;
            set => component.value = value;
        }


        [InitWithDependency]
        private void Init()
        {
            background = CreateChild<UguiSprite>("background");
            foregroundArea = CreateChild<UguiObject>("foreground-area");
            foreground = foregroundArea.CreateChild<UguiSprite>("foreground");
            thumbArea = CreateChild<UguiObject>("thumb-area");
            thumb = thumbArea.CreateChild<UguiSprite>("thumb");

            background.ImageType = Image.Type.Sliced;
            background.Anchor = Anchors.Fill;
            background.RawSize = Vector2.zero;
            background.Depth = -2;

            foregroundArea.Anchor = Anchors.Fill;
            foregroundArea.Position = Vector2.zero;
            foregroundArea.RawSize = Vector2.zero;
            foregroundArea.Depth = -1;

            foreground.ImageType = Image.Type.Sliced;
            foreground.Anchor = Anchors.LeftStretch;
            foreground.RawSize = Vector2.zero;

            thumbArea.Anchor = Anchors.Fill;
            thumbArea.Position = Vector2.zero;
            thumbArea.RawSize = Vector2.zero;

            thumb.Anchor = Anchors.LeftStretch;

            component.targetGraphic = thumb.GetComponent<Image>();
            component.fillRect = foreground.RawTransform;
            component.handleRect = thumb.RawTransform;

            Size = new Vector2(200, 36);
        }

        public void SetNoTransition() => component.SetNoTransition();

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled) => component.SetSpriteSwapTransition(highlight, selected, pressed, disabled);

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration) => component.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);

        public void SetColorTintTransition(Color normal, float duration) => component.SetColorTintTransition(normal, duration);

    }
}