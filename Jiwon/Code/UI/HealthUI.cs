using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Work.Jiwon.Code.EventSystem;
using Work.JW.Code.Core.EventSystems;

namespace Work.Jiwon.Code.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private HealthImage healthPrefab;

        private List<HealthImage> _maxHealthImages;
        
        private void Awake()
        {
            _maxHealthImages = new List<HealthImage>();
            playerChannel.AddListener<PlayerHitEvent>(HandleHitEvent);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerHitEvent>(HandleHitEvent);
        }

        private void HandleHitEvent(PlayerHitEvent evt)
        {
            if (_maxHealthImages.Count < evt.maxHeath)
            {
                int cont = (int)evt.maxHeath - _maxHealthImages.Count;
                for (int i = 0; i < cont; i++)
                {
                    HealthImage image = Instantiate(healthPrefab, transform);
                    image.InitImage();
                    image.IsEnable = true;
                    _maxHealthImages.Add(image);
                }
            }
            
            if (_maxHealthImages.Count != evt.heath)
            {
                for (int i = 0; i < _maxHealthImages.Count; i++)
                {
                    if (i < evt.heath)
                    {
                        _maxHealthImages[i].IsEnable = true;
                    }
                    else
                        _maxHealthImages[i].IsEnable = false;
                }
            }
        }
        
    }
}