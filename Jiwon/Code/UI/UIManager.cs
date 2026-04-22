using System;
using UnityEngine;
using Work.Jiwon.Code.EventSystem;
using Work.JW.Code.Core.EventSystems;

namespace Work.Jiwon.Code.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private GameEventChannelSO levelChannel;
        [SerializeField] private StoreSystem storeUI;
        [SerializeField] private CanvasGroup gameEndUI;
 
        private void Awake()
        {
            playerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
            levelChannel.AddListener<LevelUpEvent>(HandleLevelUpEvent);
            storeUI.gameObject.SetActive(false);
            // gameEndUI.gameObject.SetActive(false);
            gameEndUI.alpha = 0;
            gameEndUI.interactable = false;
            gameEndUI.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
            levelChannel.RemoveListener<LevelUpEvent>(HandleLevelUpEvent);
        }

        private void HandlePlayerDeadEvent(PlayerDeadEvent evt)
        {
            // gameEndUI.gameObject.SetActive(true);
            gameEndUI.alpha = 1;
            gameEndUI.interactable = true;
            gameEndUI.blocksRaycasts = true;
        }

        private void HandleLevelUpEvent(LevelUpEvent evt)
        {
            storeUI.gameObject.SetActive(true);
        }
    }
}