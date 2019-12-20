using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics.UI
{
    public class UguiDropdown : UguiButton, IDropdown {

        public event Action<int> OnSelected;

        protected int value;
        protected float fadeDuration;
        protected UguiDropdownProperty property;
        protected List<Dropdown.OptionData> options;

        protected UguiScrollView popup;
        protected GridLayoutGroup popupGrid;
        protected UguiScrollBar popupScrollbar;

        private UguiButton blocker;


        public IDropdownProperty Property => property;

        public List<Dropdown.OptionData> Options => options;

        public int Value
        {
            get => value;
            set => SetValue(value);
        }

        public float FadeDuration
        {
            get => fadeDuration;
            set => fadeDuration = value;
        }


        [InitWithDependency]
        private void Init()
        {
            property = new UguiDropdownProperty();
            options = new List<Dropdown.OptionData>();

            popup = CreateChild<UguiScrollView>("popup", DepthPresets.DropdownPopup);
            popupGrid = popup.Container.RawObject.AddComponent<GridLayoutGroup>();
            popupScrollbar = popup.CreateChild<UguiScrollBar>("scroll-bar", DepthPresets.DropdownPopup + 1);

            // Init property
            property.ScrollView = popup;

            // Init popup
            var popupCanvas = popup.Canvas = popup.RawObject.AddComponent<Canvas>();
            popupCanvas.overrideSorting = true;
            popupCanvas.sortingOrder = DepthPresets.DropdownPopup;
            var popupRaycaster = popup.RawObject.AddComponent<GraphicRaycaster>();
            popupRaycaster.ignoreReversedGraphics = true;

            popup.Anchor = Anchors.BottomStretch;
            popup.Pivot = Pivots.Top;
            popup.RawSize = new Vector2(0f, 100f);
            popup.IsHorizontal = false;
            popup.VerticalScrollbar = popupScrollbar;
            popup.Active = false;

            // Init popup scrollbar
            popupScrollbar.Anchor = Anchors.RightStretch;
            popupScrollbar.Pivot = Pivots.TopRight;
            popupScrollbar.OffsetTop = 0f;
            popupScrollbar.OffsetBottom = 0f;
            popupScrollbar.Width = 4f;
            popupScrollbar.X = 0;
            popupScrollbar.Direction = Scrollbar.Direction.BottomToTop;

            // Init self
            Size = new Vector2(200, 36);
            OnClick += Show;
        }

        public void Show()
        {
            CreateBlocker();
            ShowPopup();
        }

        public void Hide()
        {
            DestroyBlocker();
            HidePopup();
        }

        /// <summary>
        /// Creates a blocker to prevent clicking other areas while dropdown popup is enabled.
        /// </summary>
        private void CreateBlocker()
        {
            if(blocker != null) return;

            blocker = Root.CreateChild<UguiButton>("blocker", DepthPresets.PopupBlocker);

            var blockerCanvas = blocker.Canvas = blocker.RawObject.AddComponent<Canvas>();
            blockerCanvas.overrideSorting = true;
            blockerCanvas.sortingOrder = DepthPresets.PopupBlocker;

            var raycaster = blocker.RawObject.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;

            blocker.Anchor = Anchors.Fill;
            blocker.RawSize = Vector2.zero;

            blocker.Background.Color = new Color();
            blocker.OnClick += Hide;
        }

        /// <summary>
        /// Destroys the current blocker, if exists.
        /// </summary>
        private void DestroyBlocker()
        {
            if(blocker == null) return;
            
            blocker.Destroy();
        }

        /// <summary>
        /// Handles popup showing process.
        /// </summary>
        private void ShowPopup()
        {
            // TODO: Use anime
            popup.Active = true;

            // Create cells
            var container = popup.Container;
            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                var entry = container.CreateChild<UguiToggle>("entry", i);
                entry.Value = i == Value;
                entry.Label.Text = option.text;
                property.SetupCell(entry);

                int index = i;
                entry.OnChange += (value) => Value = index;
            }

            // Setup grid
            property.SetupGrid(popupGrid);
            property.SetupScrollView(options.Count);

            // Reposition the container so the selected entry would be shown.
        }

        /// <summary>
        /// Handles popup hiding process.
        /// </summary>
        private void HidePopup()
        {
            // TODO: Use anime
            popup.Active = false;

            // Destroy all existing cells
            var container = popup.Container.RawTransform;
            for (int i = container.childCount - 1; i >= 0; i--)
                container.GetChild(i).GetComponent<UguiToggle>()?.Destroy();
        }

        /// <summary>
        /// Internally sets the value of the dropdown.
        /// </summary>
        private void SetValue(int value)
        {
            // Clamp value
            value = Mathf.Clamp(value, 0, options.Count);
            this.value = value;
            OnSelected?.Invoke(value);

            // Change label
            if (value < options.Count)
            {
                label.Text = options[value].text;
            }
            else
            {
                label.Text = "";
            }

            // Hide popup
            Hide();
        }
    }
}