using UnityEngine;
using TMPro;

public class ScoreUIScript : MonoBehaviour
{
    private TMP_Text scoreUI;

    private void Awake()
    {
        scoreUI = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        ScoreManager.Instance.OnScoreChange += SetScoreUI;
    }
    private void SetScoreUI(int score)
    {
        scoreUI.SetText(score.ToString());
    }
}
