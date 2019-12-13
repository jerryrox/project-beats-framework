using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI
{
    public class UguiInputBox : UguiObject<InputField>, IInputBox {

        public event Action<string> OnChanged;
        public event Action<string> OnSubmitted;

        private UguiLabel placeholderLabel;
        private UguiLabel valueLabel;
        private UguiSprite backgroundSprite;

        private CanvasGroup canvasGroup;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public Color Color
        {
            get => valueLabel.Color;
            set => valueLabel.Color = placeholderLabel.Color = value;
        }

        public ILabel PlaceholderLabel => placeholderLabel;

        public ILabel ValueLabel => valueLabel;

        public ISprite Background => backgroundSprite;

        public string Text
        {
            get => component.text;
            set => component.text = value;
        }

        public string Placeholder
        {
            get => placeholderLabel.Text;
            set => placeholderLabel.Text = value;
        }

        public int CharacterLimit
        {
            get => component.characterLimit;
            set => component.characterLimit = value;
        }

        public Color SelectionColor
        {
            get => component.selectionColor;
            set => component.selectionColor = value;
        }

        public InputField.LineType LineType
        {
            get => component.lineType;
            set => component.lineType = value;
        }

        public InputField.InputType InputType
        {
            get => component.inputType;
            set => component.inputType = value;
        }

        public TouchScreenKeyboardType KeyboardType
        {
            get => component.keyboardType;
            set => component.keyboardType = value;
        }

        public InputField.CharacterValidation ValidationType
        {
            get => component.characterValidation;
            set => component.characterValidation = value;
        }




        protected override void Awake()
        {
            base.Awake();

            canvasGroup = myObject.AddComponent<CanvasGroup>();
        }

        [InitWithDependency]
        private void Init()
        {
            backgroundSprite = CreateChild<UguiSprite>("background");
            placeholderLabel = CreateChild<UguiLabel>("placeholder");
            valueLabel = CreateChild<UguiLabel>("value");

            backgroundSprite.Anchor = Anchors.Fill;
            backgroundSprite.Size = this.Size;

            placeholderLabel.IsItalic = true;
            placeholderLabel.WrapText = true;
            placeholderLabel.Anchor = Anchors.Fill;
            placeholderLabel.Width = this.Width - 20;
            placeholderLabel.Height = this.Height - 14;
            placeholderLabel.Alignment = TextAnchor.MiddleLeft;

            valueLabel.WrapText = true;
            valueLabel.Width = placeholderLabel.Width;
            valueLabel.Height = placeholderLabel.Height;
            valueLabel.Alignment = TextAnchor.MiddleLeft;

            component.targetGraphic = backgroundSprite.GetComponent<Image>();
            component.textComponent = valueLabel.GetComponent<Text>();
            component.placeholder = placeholderLabel.GetComponent<Text>();
            component.contentType = InputField.ContentType.Custom;
            component.onEndEdit.AddListener((value) => OnSubmitted?.Invoke(value));
            component.onValueChanged.AddListener((value) => OnChanged?.Invoke(value));

            Size = new Vector2(180, 36);
            LineType = InputField.LineType.SingleLine;
            InputType = InputField.InputType.Standard;
            KeyboardType = TouchScreenKeyboardType.Default;
            ValidationType = InputField.CharacterValidation.None;
        }

        public void SetNoTransition() => component.SetNoTransition();

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled)
            => component.SetSpriteSwapTransition(highlight, selected, pressed, disabled);

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration)
            => component.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);

        public void SetColorTintTransition(Color normal, float duration)
            => component.SetColorTintTransition(normal, duration);
    }
}