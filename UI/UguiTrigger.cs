using System;
using PBFramework.Graphics;
using UnityEngine.EventSystems;

namespace PBFramework.UI
{
    public class UguiTrigger : UguiObject<EventTrigger>, ITrigger {

        public event Action OnPointerEnter;
        public event Action OnPointerExit;
        public event Action OnPointerDown;
        public event Action OnPointerUp;
        public event Action OnPointerClick;


        protected override void Awake()
        {
            base.Awake();

            AddEntry(EventTriggerType.PointerEnter, () => OnPointerEnter?.Invoke());
            AddEntry(EventTriggerType.PointerExit, () => OnPointerEnter?.Invoke());
            AddEntry(EventTriggerType.PointerDown, () => OnPointerDown?.Invoke());
            AddEntry(EventTriggerType.PointerUp, () => OnPointerUp?.Invoke());
            AddEntry(EventTriggerType.PointerClick, () => OnPointerClick?.Invoke());
        }   
        
        protected void AddEntry(EventTriggerType type, Action callback)
        {
            var entry = new EventTrigger.Entry()
            {
                eventID = type,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(delegate { callback(); });
            component.triggers.Add(entry);
        }
    }
}