using System;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PBFramework.UI
{
    public class UguiTrigger : UguiObject<Image>, ITrigger,
        IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnPointerEnter;
        public event Action OnPointerExit;
        public event Action OnPointerDown;
        public event Action OnPointerUp;
        public event Action OnPointerClick;


        [InitWithDependency]
        private void Init()
        {
            component.SetAlpha(0f);
        }

        public virtual void InvokeEnter() => OnPointerEnter?.Invoke();

        public virtual void InvokeExit() => OnPointerExit?.Invoke();

        public virtual void InvokeDown() => OnPointerDown?.Invoke();

        public virtual void InvokeUp() => OnPointerUp?.Invoke();

        public virtual void InvokeClick() => OnPointerClick?.Invoke();

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
    }
}