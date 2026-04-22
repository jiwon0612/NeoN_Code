using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Work.JW.Code.Core.EventSystems;

namespace Work.Jiwon.Code.UI
{
    public class ExitGameUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameEventChannelSO[] events;
        [SerializeField] private float delay;
        [SerializeField] private Player.Player player;
        private float _timer;
        
        private void Awake()
        {
            _timer = 0;
        }

        private void Update()
        {
            if (player.IsDead) return;
            
            image.fillAmount = _timer / delay;
            if (Input.GetKey(KeyCode.Escape))
            {
                _timer += Time.deltaTime;
                if (_timer >= delay)
                {
                    foreach (var itemEvent in events)
                    {
                        itemEvent.Clear();
                    }
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                _timer = 0;
            }
        }
    }
}