using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Fonts;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBFramework.UI
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

        public IFont Font
        {
            get => valueLabel.Font;
            set => valueLabel.Font = placeholderLabel.Font = value;
        }

        public bool IsBold
        {
            get => valueLabel.IsBold;
            set => valueLabel.IsBold = placeholderLabel.IsBold = value;
        }

        public bool IsItalic
        {
            get => valueLabel.IsItalic;
            set => valueLabel.IsItalic = placeholderLabel.IsItalic = value;
        }

        public bool WrapText
        {
            get => valueLabel.WrapText;
            set => valueLabel.WrapText = placeholderLabel.WrapText = value;
        }

        public int FontSize
        {
            get => valueLabel.FontSize;
            set => valueLabel.FontSize = placeholderLabel.FontSize = value;
        }

        public TextAnchor Alignment
        {
            get => valueLabel.Alignment;
            set => valueLabel.Alignment = placeholderLabel.Alignment = value;
        }

        public string Text
        {
            get => component.text;
            set => component.text = value;
        }

        public ILabel PlaceholderLabel => placeholderLabel;

        public ILabel ValueLabel => valueLabel;

        public ISprite Background => backgroundSprite;

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
            backgroundSprite = AddComponentInject<UguiSprite>();
            placeholderLabel = CreateChild<UguiLabel>("placeholder");
            valueLabel = CreateChild<UguiLabel>("value");

            valueLabel.Anchor = placeholderLabel.Anchor = Anchors.Fill;
            valueLabel.RawWidth = placeholderLabel.RawWidth = -20;
            valueLabel.RawHeight = placeholderLabel.RawHeight = -14;

            component.targetGraphic = backgroundSprite.GetComponent<Image>();
            component.textComponent = valueLabel.GetComponent<Text>();
            component.placeholder = placeholderLabel.GetComponent<Text>();
            component.contentType = InputField.ContentType.Custom;
            component.onEndEdit.AddListener((value) => OnSubmitted?.Invoke(value));
            component.onValueChanged.AddListener((value) => OnChanged?.Invoke(value));

            Size = new Vector2(180, 36);
            WrapText = true;
            Alignment = TextAnchor.UpperLeft;
            LineType = InputField.LineType.SingleLine;
            InputType = InputField.InputType.Standard;
            KeyboardType = TouchScreenKeyboardType.Default;
            ValidationType = InputField.CharacterValidation.None;

            SetNoTransition();
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