using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiScrollBar : UguiObject<Scrollbar>, IScrollBar {

        public event Action<float> OnChange;

        protected UguiSprite background;
        protected UguiObject slideArea;
        protected UguiSprite foreground;


        public ISprite Background => background;

        public ISprite Foreground => foreground;

        public Scrollbar.Direction Direction
        {
            get => component.direction;
            set => component.direction = value;
        }

        public float Value
        {
            get => component.value;
            set => component.value = value;
        }

        public float ForegroundSize
        {
            get => component.size;
            set => component.size = value;
        }

        public int Steps
        {
            get => component.numberOfSteps;
            set => component.numberOfSteps = value;
        }


        [InitWithDependency]
        private void Init()
        {
            background = AddComponentInject<UguiSprite>();
            slideArea = CreateChild<UguiObject>("slide-area");
            foreground = slideArea.CreateChild<UguiSprite>("foreground");

            background.ImageType = Image.Type.Sliced;
            background.Color = Color.black;

            slideArea.Anchor = AnchorType.Fill;
            slideArea.RawSize = Vector2.zero;

            foreground.OnSpriteChange += OnForegroundSpriteChange;
            foreground.ImageType = Image.Type.Sliced;
            foreground.Anchor = AnchorType.Fill;
            foreground.RawSize = Vector2.zero;

            component.onValueChanged.AddListener((value) => OnChange?.Invoke(value));
            component.targetGraphic = foreground.GetComponent<Image>();
            component.handleRect = foreground.RawTransform;
            
            Direction = Scrollbar.Direction.LeftToRight;
        }

        public void SetNoTransition() => component.SetNoTransition();

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled) => component.SetSpriteSwapTransition(highlight, selected, pressed, disabled);

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration) => component.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);

        public void SetColorTintTransition(Color normal, float duration) => component.SetColorTintTransition(normal, duration);

        /// <summary>
        /// Event called from foreground when its sprite has been changed.
        /// </summary>
        protected virtual void OnForegroundSpriteChange(Sprite sprite)
        {
            var border = sprite == null ? Vector4.zero : sprite.border;
            slideArea.Offset = new Offset(border.x, border.y, border.z, border.w);
            foreground.Offset = new Offset(-border.x, -border.y, -border.z, -border.w);
        }
    }
}