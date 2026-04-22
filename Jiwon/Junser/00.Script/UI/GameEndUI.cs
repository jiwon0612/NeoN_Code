using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.Jiwon.Code.EventSystem;
using Work.JW.Code.Core.EventSystems;
using Random = UnityEngine.Random;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO playerChannel;
    [SerializeField] private GameEventChannelSO scoreChannel;
    [SerializeField] private List<GameEventChannelSO> channels;
    
    [SerializeField] private TMP_Text _scoreResult;
    [SerializeField] private TMP_Text _gameEndtext;
    [SerializeField] private TMP_Text saveDataText;

    [SerializeField] private Transform gameOverPanelTrm;
    
    [SerializeField] private List<string> ments;

    private void Awake()
    {
        playerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
        scoreChannel.AddListener<SaveUserScoreEvent>(HandleSaveUserScoreEvent);
        scoreChannel.AddListener<ChangeHighScoreEvent>(HandleCurrentScoreEvent);

        saveDataText.gameObject.SetActive(false);
    }

    private void HandlePlayerDeadEvent(PlayerDeadEvent evt)
    {
        gameOverPanelTrm.gameObject.SetActive(true);
        SetGameOverResultText();
    }

    private void HandleCurrentScoreEvent(ChangeHighScoreEvent evt)
    {
        _scoreResult.text = $"Score : {evt.score}";
    }
    
    private void HandleSaveUserScoreEvent(SaveUserScoreEvent evt)
    {
        saveDataText.gameObject.SetActive(!evt.isCompleted);
    }

    private void SetGameOverResultText()
    {
        _gameEndtext.SetText(ments[Random.Range(0, ments.Count)]);
    }
    public void RestartGame()
    {
        foreach (var channel in channels)
            channel.Clear();
        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }
    public void ToTitle()
    {
        foreach (var channel in channels)
            channel.Clear();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
        scoreChannel.RemoveListener<ChangeHighScoreEvent>(HandleCurrentScoreEvent);
    }
}
