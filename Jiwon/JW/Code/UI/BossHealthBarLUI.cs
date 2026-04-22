using DG.Tweening;
using UnityEngine;
using Work.Jiwon.Code.Entities;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;

namespace Work.JW.Code.UI
{
    public class BossHealthBarLUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO gameChannel;
        [SerializeField] private Transform barBackgroundTrm;
        [SerializeField] private Transform bossTextTrm;
        private Transform _healthBarUITrm;

        private Entity _boss;

        private void Awake()
        {
            _healthBarUITrm = barBackgroundTrm.GetChild(0);
            barBackgroundTrm.gameObject.SetActive(false);
            bossTextTrm.gameObject.SetActive(false);
            
            gameChannel.AddListener<BossSpawnEvent>(HandleBossSpawnEvent);
        }

        private void OnDestroy()
        {
            gameChannel.RemoveListener<BossSpawnEvent>(HandleBossSpawnEvent);
        }

        private void HandleBossSpawnEvent(BossSpawnEvent evt)
        {
            _boss = evt.boss;
            _boss.GetCompo<EntityHealth>().OnHealthChangedEvent.AddListener(HandleHealthChangeEvent);
            _boss.OnDeadEvent.AddListener(HandleBossDeathEvent);
            
            barBackgroundTrm.gameObject.SetActive(true);
            bossTextTrm.gameObject.SetActive(true);
        }

        private void HandleHealthChangeEvent(float curHealth, float curMax)
        {
            float healthPercent = Mathf.Clamp(curHealth / curMax, 0, 1);
            
            _healthBarUITrm.DOScaleX(healthPercent, 0.5f).SetEase(Ease.OutQuint);
        }

        private void HandleBossDeathEvent()
        {
            barBackgroundTrm.gameObject.SetActive(false);
            bossTextTrm.gameObject.SetActive(false);
        }
    }
}