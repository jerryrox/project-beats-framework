using System;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBFramework.UI
{
    public class UguiTrigger : UguiObject<Image>, ITrigger,
        IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,
        IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public event Action OnPointerEnter;
        public event Action OnPointerExit;
        public event Action OnPointerDown;
        public event Action OnPointerUp;
        public event Action OnPointerClick;

        public event Action<ICursor> OnDragStart;
        public event Action<ICursor> OnDragging;
        public event Action<ICursor> OnDragEnd;

        private IInputManager inputManager;


        [InitWithDependency]
        private void Init()
        {
            inputManager = base.InputManager;

            component.SetAlpha(0f);
        }

        public virtual void InvokeEnter() => OnPointerEnter?.Invoke();

        public virtual void InvokeExit() => OnPointerExit?.Invoke();

        public virtual void InvokeDown() => OnPointerDown?.Invoke();

        public virtual void InvokeUp() => OnPointerUp?.Invoke();

        public virtual void InvokeClick() => OnPointerClick?.Invoke();

        /// <summary>
        /// Returns the cursor data representing the specified pointer event.
        /// </summary>
        private ICursor GetCursor(PointerEventData data)
        {
            if(data == null || inputManager == null)
                return null;
            return inputManager.FindCursor(data.position);
        }

        /// <summary>
        /// Invokes the specified pointer event after finding the corresponding cursor info.
        /// </summary>
        private void InvokePointerEventInfo(Action<ICursor> pointerEvent, PointerEventData data)
        {
            var cursor = GetCursor(data);
            if(cursor != null && pointerEvent != null)
                pointerEvent.Invoke(cursor);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            InvokeClick();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            InvokeDown();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            InvokeUp();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            InvokeEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            InvokeExit();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            InvokePointerEventInfo(OnDragStart, eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            InvokePointerEventInfo(OnDragging, eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            InvokePointerEventInfo(OnDragEnd, eventData);
        }
    }
}