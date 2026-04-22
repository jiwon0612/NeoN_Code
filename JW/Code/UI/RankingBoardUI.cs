using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.UI
{
    public class RankingBoardUI : MonoBehaviour
    {
        public UnityEvent OnRefresh;
        [SerializeField] private GameEventChannelSO scoreChannel;
        [SerializeField] private RankingBoardItemUI boardItemPrefab;
        [SerializeField] private Transform contentTrm;

        [SerializeField] private RankingBoardItemUI currentUserItem;

        private void Awake()
        {
            scoreChannel.AddListener<ChangeRankingEvent>(HandleChangeRanking);
            scoreChannel.AddListener<ChangeHighScoreEvent>(HandleChangeHighScore);
            scoreChannel.AddListener<LoadUserScoreEvent>(HandleLoadUserScore);
        }

        private void OnEnable()
        {
            RefreshRanking();
        }

        private void OnDestroy()
        {
            scoreChannel.RemoveListener<ChangeRankingEvent>(HandleChangeRanking);
            scoreChannel.RemoveListener<ChangeHighScoreEvent>(HandleChangeHighScore);
            scoreChannel.RemoveListener<LoadUserScoreEvent>(HandleLoadUserScore);
        }
        
        public void RefreshRanking()
        {
            ClearRankingBoard();
            
            scoreChannel.RaiseEvent(ScoreEvents.RefreshRankingEvent);
            OnRefresh?.Invoke();
        }
        
        public void ClearRankingBoard()
        {
            contentTrm.GetComponentsInChildren<RankingBoardItemUI>().ToList().ForEach(r => Destroy(r.gameObject));
        }

        private void HandleChangeRanking(ChangeRankingEvent evt)
        {
            int itemRanking = 1;
            Dictionary<string, int> rankings = evt.rankings;

            ClearRankingBoard(); 
            
            foreach (var ranking in rankings)
            {
                var item = Instantiate(boardItemPrefab, contentTrm);

                item.SetRankingItem(itemRanking, ranking.Key, ranking.Value);

                itemRanking++;
            }
        }
        
        private void HandleChangeHighScore(ChangeHighScoreEvent evt)
        {
            currentUserItem.SetRankingItem(0, evt.userId, evt.score);
        }
        
        private void HandleLoadUserScore(LoadUserScoreEvent evt)
        {
            currentUserItem.SetRankingItem(0, evt.userId, evt.userScore);
        }
    }
}