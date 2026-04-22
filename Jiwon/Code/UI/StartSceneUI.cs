using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.UI;

namespace Work.Jiwon.Code.UI
{
    public class StartSceneUI : MonoBehaviour
    {
        [SerializeField] private SettingUI setting;
        [SerializeField] private RankingBoardUI ranking;

        [SerializeField] private List<GameEventChannelSO> eventList;
        
        public void ChangeScene(int value)
        { 
            eventList.ForEach(e => e.Clear());
            
            SceneManager.LoadScene(value);
        }

        public void ShowSettingUI()
        {
            setting.gameObject.SetActive(true);
        }

        public void ShowRankingBoard()
        {
            ranking.gameObject.SetActive(true);
        }
        
        public void QuitGame() => Application.Quit();
    }
}