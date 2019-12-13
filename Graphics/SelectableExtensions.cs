using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public static class SelectableExtensions {

        public static void SetNoTransition(this Selectable context)
        {
            context.transition = Selectable.Transition.None;
        }

        public static void SetSpriteSwapTransition(this Selectable context, Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled)
        {
            context.transition = Selectable.Transition.SpriteSwap;
            context.spriteState = new SpriteState()
            {
                highlightedSprite = highlight,
                selectedSprite = selected,
                pressedSprite = pressed,
                disabledSprite = disabled
            };
        }

        public static void SetColorTintTransition(this Selectable context, Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration)
        {
            context.transition = Selectable.Transition.ColorTint;
            context.colors = new ColorBlock()
            {
                normalColor = normal,
                highlightedColor = highlight,
                selectedColor = selected,
                pressedColor = pressed,
                disabledColor = disabled,
            };
        }

        public static void SetColorTintTransition(this Selectable context, Color normal, float duration)
        {
            Color highlight = normal * 1.05f;
            Color selected = normal * 0.95f;
            Color pressed = normal * 0.9f;
            Color disabled = normal * 0.5f;
            highlight.a = selected.a = pressed.a = disabled.a = normal.a;

            SetColorTintTransition(context, normal, highlight, selected, pressed, disabled, duration);
        }
    }
}