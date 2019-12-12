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

        public void SetNoTransition()
        {
            button.transition = Selectable.Transition.None;
        }

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled)
        {
            button.transition = Selectable.Transition.SpriteSwap;
            button.spriteState = new SpriteState()
            {
                highlightedSprite = highlight,
                selectedSprite = selected,
                pressedSprite = pressed,
                disabledSprite = disabled
            };
        }

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration)
        {
            button.transition = Selectable.Transition.ColorTint;
            button.colors = new ColorBlock()
            {
                normalColor = normal,
                highlightedColor = highlight,
                selectedColor = selected,
                pressedColor = pressed,
                disabledColor = disabled,
            };
        }

        public void SetColorTintTransition(Color normal, float duration)
        {
            Color highlight = normal * 1.05f;
            Color selected = normal * 0.95f;
            Color pressed = normal * 0.9f;
            Color disabled = normal * 0.5f;
            highlight.a = selected.a = pressed.a = disabled.a = normal.a;

            SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);
        }
    }
}