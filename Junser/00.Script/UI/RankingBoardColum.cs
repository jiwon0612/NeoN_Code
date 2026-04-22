using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingBoardColum : MonoBehaviour
{
    [SerializeField] private TMP_Text ranking;
    [SerializeField] private TMP_Text userName;
    [SerializeField] private TMP_Text Score;

    public void SetRankingBoard(int ranking, string userName, int score)
    {
        this.ranking.SetText(ranking.ToString());
        this.userName.SetText(userName);
        this.Score.SetText(score.ToString());
    }
}
