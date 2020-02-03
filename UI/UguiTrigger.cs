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

        public void InvokeEnter() => OnPointerEnter?.Invoke();

        public void InvokeExit() => OnPointerExit?.Invoke();

        public void InvokeDown() => OnPointerDown?.Invoke();

        public void InvokeUp() => OnPointerUp?.Invoke();

        public void InvokeClick() => OnPointerClick?.Invoke();

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
    // public class UguiTrigger : UguiObject<EventTrigger>, ITrigger {

    //     public event Action OnPointerEnter;
    //     public event Action OnPointerExit;
    //     public event Action OnPointerDown;
    //     public event Action OnPointerUp;
    //     public event Action OnPointerClick;


    //     protected override void Awake()
    //     {
    //         base.Awake();

    //         AddEntry(EventTriggerType.PointerEnter, () => OnPointerEnter?.Invoke());
    //         AddEntry(EventTriggerType.PointerExit, () => OnPointerEnter?.Invoke());
    //         AddEntry(EventTriggerType.PointerDown, () => OnPointerDown?.Invoke());
    //         AddEntry(EventTriggerType.PointerUp, () => OnPointerUp?.Invoke());
    //         AddEntry(EventTriggerType.PointerClick, () => OnPointerClick?.Invoke());
    //     }

    //     public void InvokeEnter() => OnPointerEnter?.Invoke();

    //     public void InvokeExit() => OnPointerExit?.Invoke();

    //     public void InvokeDown() => OnPointerDown?.Invoke();

    //     public void InvokeUp() => OnPointerUp?.Invoke();

    //     public void InvokeClick() => OnPointerClick?.Invoke();

    //     protected void AddEntry(EventTriggerType type, Action callback)
    //     {
    //         var entry = new EventTrigger.Entry()
    //         {
    //             eventID = type,
    //             callback = new EventTrigger.TriggerEvent()
    //         };
    //         entry.callback.AddListener(delegate { callback(); });
    //         component.triggers.Add(entry);
    //     }
    // }
}