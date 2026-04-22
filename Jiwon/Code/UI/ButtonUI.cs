using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Work.Jiwon.Code.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonUI : MonoBehaviour
    {
        public UnityEvent OnClickEvent;
        
        [SerializeField] private float upSize;
        [SerializeField] private float tweenDuration;

        private float _defaultSize;
        
        private Tween _upTween;
        private RectTransform _rectTransform;
        private EventTrigger _eventTrigger;

        private void Awake()
        {
            _eventTrigger = GetComponent<EventTrigger>();
            
            _rectTransform = transform as RectTransform;
            _defaultSize = _rectTransform.localScale.x;
            
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((eventData) => MouseEnterEvent());
            
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((eventData) => MouseExitEvent());
            
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((eventData) => MouseClickEvent());
            
            _eventTrigger.triggers.Add(enterEntry);
            _eventTrigger.triggers.Add(exitEntry);
            _eventTrigger.triggers.Add(clickEntry);
        }

        public void MouseEnterEvent()
        {
            if (_upTween.IsActive())
                _upTween.Complete();
            
            _upTween =_rectTransform.DOScale(upSize, tweenDuration);
        }

        public void MouseExitEvent()
        {
            if (_upTween.IsActive())
                _upTween.Complete();
            
            _upTween = _rectTransform.DOScale(_defaultSize, tweenDuration);
        }

        public void MouseClickEvent()
        {
            OnClickEvent?.Invoke();
        }
    }
}