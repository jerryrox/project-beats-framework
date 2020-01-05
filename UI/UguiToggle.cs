using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiToggle : UguiObject<Toggle>, IToggle {

        public event Action<bool> OnChange;

        protected CanvasGroup canvasGroup;

        protected UguiSprite background;
        protected UguiSprite tick;
        protected UguiLabel label;


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

        public ILabel Label => label;


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();
            background = CreateChild<UguiSprite>("background");
            tick = CreateChild<UguiSprite>("tick", 1);
            label = CreateChild<UguiLabel>("label", 2);

            background.ImageType = Image.Type.Sliced;
            background.Size = new Vector2(36, 36);

            tick.Size = new Vector2(30, 30);

            label.Pivot = Pivots.Left;
            label.Alignment = TextAnchor.MiddleLeft;
            label.Position = new Vector2(20, 0);
            label.Size = new Vector2(200, 36);

            component.onValueChanged.AddListener((value) => OnChange?.Invoke(value));
            component.targetGraphic = background.GetComponent<Image>();
            component.graphic = tick.GetComponent<Image>();

            Value = false;
            Size = new Vector2(36f, 36f);
        }

        public void SetNoTransition() => component.SetNoTransition();

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled) => component.SetSpriteSwapTransition(highlight, selected, pressed, disabled);

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration) => component.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);

        public void SetColorTintTransition(Color normal, float duration) => component.SetColorTintTransition(normal, duration);
    }
}