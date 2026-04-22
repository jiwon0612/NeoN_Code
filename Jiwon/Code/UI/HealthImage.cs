using System;
using UnityEngine;
using UnityEngine.UI;

namespace Work.Jiwon.Code.UI
{
    public class HealthImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private Action OnEnableEvent;
        private Action OnDisableEvent;

        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value)
                    OnEnableEvent?.Invoke();
                else
                    OnDisableEvent?.Invoke();

                _isEnable = value;
            }
        }

        public void InitImage()
        {
            OnEnableEvent += HandleEnableEvent;
            OnDisableEvent += HandleDisableEvent;
        }

        private void HandleEnableEvent()
        {
            _image.enabled = true;
        }

        private void HandleDisableEvent()
        {
            _image.enabled = false;
        }
    }
}