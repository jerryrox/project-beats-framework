using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public class UguiDropdownTemplate : UguiObject, IDropdownTemplate {

        protected Dropdown dropdown;

        protected UguiObject viewport;
        protected UguiObject content;
        protected UguiObject item;
        protected UguiObject scrollbar;


        public GameObject CellObject => Item.gameObject;

        public GameObject CellBackground => ItemBackground.gameObject;

        public GameObject CellCheckmark => ItemCheckmark.gameObject;

        public GameObject CellLabel => ItemLabel.gameObject;

        public Image Background { get; protected set; }

        public ScrollRect ScrollRect { get; protected set; }

        public Mask ViewportMask { get; protected set; }

        public Toggle Item { get; protected set; }

        public Image ItemBackground { get; protected set; }

        public Image ItemCheckmark { get; protected set; }

        public Text ItemLabel { get; protected set; }


        public virtual void InitTemplate(Dropdown dropdown)
        {
            this.dropdown = dropdown;

            // Template object
            Anchor = AnchorType.BottomStretch;
            Pivot = PivotType.Top;
            Position = Vector2.zero;
            RawSize = new Vector2(0f, 200f);

            var background = Background = myObject.AddComponent<Image>();
            background.type = Image.Type.Sliced;

            var scrollRect = ScrollRect = myObject.AddComponent<ScrollRect>();
            // scrollRect.content
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            // Viewport object

            // Content object

            // Item object

            // Item children


            // var parent = Parent;
            // if (parent != null)
            // {
            //     Anchor = Anchors.BottomStretch;
            //     Pivot = Pivots.Top;

            //     Position = Vector2.zero;
            //     RawSize = Vector2.zero;
            // }

            // var cell = container.CreateChild<UguiToggle>("item-cell");

            // cell.Anchor = Anchors.MiddleStretch;
            // SetCell(cell, null);
        }
    }
}