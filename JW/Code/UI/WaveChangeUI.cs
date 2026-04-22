using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.UI
{
    public class WaveChangeUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO gameChannel;
        [SerializeField] private TextMeshProUGUI waveText;

        [SerializeField] private float duration = 2f;
        [SerializeField] private float moveDistance = -100f;

        private Transform waveTextTrm;
        private float startPosY;


        private void Awake()
        {
            waveTextTrm = waveText.transform;
            startPosY = transform.position.y;

            gameChannel.AddListener<WaveChangeEvent>(HandleChangeWave);
        }

        private void OnDestroy()
        {
            gameChannel.RemoveListener<WaveChangeEvent>(HandleChangeWave);
        }

        private void HandleChangeWave(WaveChangeEvent evt)
        {
            if (evt.wave >= 0)
                waveText.text = $"Wave {evt.wave + 1}";
            else
                waveText.text = $"Infinite Mode";
            
            StartCoroutine(MoveWaveText());
        }

        private IEnumerator MoveWaveText()
        {
            waveTextTrm.DOMoveY(startPosY + moveDistance, duration).SetEase(Ease.OutCubic).SetUpdate(true);
            yield return new WaitForSecondsRealtime(2f);
            waveTextTrm.DOMoveY(startPosY, duration).SetEase(Ease.InBack).SetUpdate(true);
        }

        private void OnDrawGizmos()
        {
            if (waveTextTrm != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(waveTextTrm.position, waveTextTrm.position + Vector3.up * moveDistance);
            }
        }
    }
}