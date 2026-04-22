using System;
using UnityEngine;
using Work.Jiwon.Code.EventSystem;
using Work.JW.Code.Core.EventSystems;

public class CursorManager : MonoBehaviour
{
    [SerializeField] GameEventChannelSO GameCHannel;
    [SerializeField] GameEventChannelSO playerChannel;

    private void Awake()
    {
        GameCHannel.AddListener<GameStopEvent>(HandleGameStopEvent);
        playerChannel.AddListener<PlayerDeadEvent>(HandleGameStopEvent);
    }

    private void HandleGameStopEvent(GameStopEvent evt)
    {
        Cursor.visible = evt.isStop;
    }
    private void HandleGameStopEvent(PlayerDeadEvent evt)
    {
        Cursor.visible = true;
    }
    private void Start()
    {
        Cursor.visible = false;
    }
}
