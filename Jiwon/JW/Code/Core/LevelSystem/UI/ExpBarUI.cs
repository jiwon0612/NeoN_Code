using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.Core.LevelSystem.UI
{
    public class ExpBarUI : MonoBehaviour
    {
        [SerializeField] private Image expBarImage;
        [SerializeField] private GameEventChannelSO levelChannel;

        private float _provExpBarPer;

        private void Awake()
        {
            levelChannel.AddListener<EXPChangeEvent>(HandleChangeExp);

            expBarImage.transform.localScale = new Vector3(0, 1, 1);
        }

        private void HandleChangeExp(EXPChangeEvent evt)
        {
            float expPercent = evt.exp / evt.wantExp;

            expBarImage.transform.DOScaleX(expPercent, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        }

        private void OnDestroy()
        {
            levelChannel.RemoveListener<EXPChangeEvent>(HandleChangeExp);
        }
    }
}