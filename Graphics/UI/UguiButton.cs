using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI
{
    public class UguiButton : UguiObject, IButton {

        public event Action OnClick;

        protected UguiLabel label;
        protected UguiSprite sprite;

        protected Button button;


        public ILabel Label => label;

        public ISprite Background => sprite;


        [InitWithDependency]
        private void Init()
        {
            sprite = CreateChild<UguiSprite>();
            label = CreateChild<UguiLabel>();

            button = myObject.AddComponent<Button>();
            button.targetGraphic = sprite.GetComponent<Image>();
            button.onClick.AddListener(() => OnClick?.Invoke());

            SetNoTransition();
        }

        public void SetNoTransition() => button.SetNoTransition();

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled)
            => button.SetSpriteSwapTransition(highlight, selected, pressed, disabled);

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration)
            => button.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);

        public void SetColorTintTransition(Color normal, float duration)
            => button.SetColorTintTransition(normal, duration);
    }
}