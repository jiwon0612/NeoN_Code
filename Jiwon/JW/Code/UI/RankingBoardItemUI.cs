using System;
using TMPro;
using UnityEngine;

namespace Work.JW.Code.UI
{
    public class RankingBoardItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemIDText;
        [SerializeField] private TextMeshProUGUI itemScoreText;

        public void SetRankingItem(int itemRanking, string itemId, int score)
        {
            if(itemRanking > 0)
                itemIDText.text = $"{itemRanking}. {itemId}";
            else
                itemIDText.text = $"{itemId}";
            itemScoreText.text = score.ToString();
        }
    }
}