using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Fonts;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public class UguiDropdownProperty : IDropdownProperty {

        /// <summary>
        /// Sets dropdown entry's transition effects.
        /// </summary>
        private Action<UguiToggle> setEntryTransition;


        public float MaxHeight { get; set; } = 300;

        public float EntryHeight { get; set; } = 24;

        public ISprite PopupBackground => ScrollView.Background;

        public IScrollBar PopupScrollbar => ScrollView.VerticalScrollbar;

        public int FontSize { get; set; } = 20;

        public IFont Font { get; set; }

        public Color LabelColor { get; set; } = new Color(0.25f, 0.25f, 0.25f, 1f);

        public Color SelectedColor { get; set; } = new Color(0.8f, 0.8f, 0.8f, 1f);

        /// <summary>
        /// The scrollview which the property will apply on.
        /// </summary>
        public UguiScrollView ScrollView { get; set; }


        public UguiDropdownProperty()
        {
            SetNoTransition();
        }

        /// <summary>
        /// Initializes the grid to use the specified properties.
        /// </summary>
        public void SetupGrid(GridLayoutGroup grid)
        {
            grid.cellSize = new Vector2(ScrollView.Container.Width, EntryHeight);
            grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            grid.startAxis = GridLayoutGroup.Axis.Vertical;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 1;
            grid.childAlignment = TextAnchor.UpperCenter;
        }

        /// <summary>
        /// Initializes the scrollview to use the specified properties.
        /// </summary>
        public void SetupScrollView(int cellCount)
        {
            float totalHeight = cellCount * EntryHeight;
            ScrollView.Container.Height = totalHeight;
            ScrollView.Height = Mathf.Min(totalHeight, MaxHeight);
        }

        /// <summary>
        /// Initializes the cell to be ready for displaying.
        /// </summary>
        public void SetupCell(UguiToggle toggle)
        {
            var background = toggle.Background;
            background.Anchor = AnchorType.Fill;
            background.Position = Vector2.zero;
            background.RawSize = Vector2.zero;

            var tick = toggle.Tick;
            tick.Anchor = AnchorType.Fill;
            tick.Position = Vector2.zero;
            tick.RawSize = Vector2.zero;
            tick.Color = SelectedColor;

            var label = toggle.Label;
            label.Anchor = AnchorType.Fill;
            label.Alignment = TextAnchor.MiddleCenter;
            label.Pivot = PivotType.Center;
            label.Position = Vector2.zero;
            label.RawSize = Vector2.zero;
            label.WrapText = true;
            label.FontSize = FontSize;
            label.Font = Font;
            label.Color = LabelColor;

            setEntryTransition.Invoke(toggle);
        }

        public void SetNoTransition()
        {
            setEntryTransition = (toggle) => toggle.SetNoTransition();
        }

        public void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled)
        {
            setEntryTransition = (toggle) => toggle.SetSpriteSwapTransition(highlight, selected, pressed, disabled);
        }

        public void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration)
        {
            setEntryTransition = (toggle) => toggle.SetColorTintTransition(normal, highlight, selected, pressed, disabled, duration);
        }

        public void SetColorTintTransition(Color normal, float duration)
        {
            setEntryTransition = (toggle) => toggle.SetColorTintTransition(normal, duration);
        }
    }
}