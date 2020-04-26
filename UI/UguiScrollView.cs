using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiScrollView : UguiObject<ScrollRect>, IScrollView {

        protected CanvasGroup canvasGroup;

        protected UguiSprite background;
        protected UguiSprite viewport;
        protected Mask viewportMask;
        protected UguiObject container;

        protected UguiScrollBar horizontalScrollbar;
        protected UguiScrollBar verticalScrollbar;

        private IAnime scrollAni;
        private Vector2 scrollTarget;

        private Image viewportImage;


        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public bool UseMask
        {
            get => viewportMask.enabled;
            set => viewportMask.enabled = viewportImage.enabled = value;
        }

        public bool ShowMaskingSprite
        {
            get => viewportMask.showMaskGraphic;
            set => viewportMask.showMaskGraphic = value;
        }

        public bool IsHorizontal
        {
            get => component.horizontal;
            set => component.horizontal = value;
        }

        public bool IsVertical
        {
            get => component.vertical;
            set => component.vertical = value;
        }

        public bool UseInertia
        {
            get => component.inertia;
            set => component.inertia = value;
        }

        public float InertiaRate
        {
            get => component.decelerationRate;
            set => component.decelerationRate = value;
        }

        public float Elasticity
        {
            get => component.elasticity;
            set => component.elasticity = value;
        }

        public ISprite Background => background;

        public ScrollRect.MovementType Movement
        {
            get => component.movementType;
            set => component.movementType = value;
        }

        public IScrollBar HorizontalScrollbar
        {
            get => horizontalScrollbar;
            set
            {
                if (!(value is UguiScrollBar scrollBar))
                    throw new ArgumentException($"Value is not a type of ({nameof(UguiScrollBar)})");
                horizontalScrollbar = scrollBar;
                component.horizontalScrollbar = scrollBar.GetComponent<Scrollbar>();
            }
        }

        public IScrollBar VerticalScrollbar
        {
            get => verticalScrollbar;
            set
            {
                if (!(value is UguiScrollBar scrollBar))
                    throw new ArgumentException($"Value is not a type of ({nameof(UguiScrollBar)})");
                verticalScrollbar = scrollBar;
                component.verticalScrollbar = scrollBar.GetComponent<Scrollbar>();
            }
        }

        public IGraphicObject Container => container;


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();
            background = AddComponentInject<UguiSprite>();
            viewport = CreateChild<UguiSprite>("viewport");
            {
                viewportImage = viewport.GetComponent<Image>();
                viewportMask = viewport.RawObject.AddComponent<Mask>();
                container = viewport.CreateChild<UguiObject>("container");
            }

            background.ImageType = Image.Type.Sliced;

            viewport.Anchor = Anchors.Fill;
            viewport.Pivot = Pivots.TopLeft;
            viewport.RawSize = Vector2.zero;

            container.Anchor = Anchors.TopStretch;
            container.Pivot = Pivots.TopLeft;
            container.Position = Vector2.zero;
            container.RawWidth = 0f;

            component.content = container.RawTransform;
            component.viewport = viewport.RawTransform;

            ShowMaskingSprite = false;
            UseInertia = true;
            InertiaRate = 0.1f;

            scrollAni = new Anime();
            scrollAni.AnimateVector2(pos => container.Position = pos)
                .AddTime(0f, () => container.Position)
                .AddTime(0.5f, () => scrollTarget)
                .Build();
            scrollAni.AddEvent(scrollAni.Duration, () => component.StopMovement());
        }

        public virtual void ResetPosition()
        {
            container.Position = Vector2.zero;
        }

        public void ScrollTo(Vector2 position)
        {
            scrollTarget = position;
            scrollAni.PlayFromStart();
        }
    }
}