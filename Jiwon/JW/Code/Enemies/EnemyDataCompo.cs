using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Jiwon.Code.Entities;
using Work.Jiwon.Code.Player;
using Work.Jiwon.Code.StatSystem;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Enemies
{
    public class EnemyDataCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private EntityFinderSO targetFinder;
        public Transform TargetTrm { get; private set; }
        public UnityEvent<int> OnHealthValueChanged;

        [SerializeField] private EnemyStat _enemyStat;
        private EntityStat _statCompo;

        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            TargetTrm = targetFinder.target.transform;
        }

        public void InitData(EnemyDataSO data)
        {
            _statCompo.SetBaseValue(_enemyStat.healthStat, data.health);
            _statCompo.SetBaseValue(_enemyStat.speedStat, data.speed);
            _statCompo.SetBaseValue(_enemyStat.damageStat, data.damage);
            _statCompo.SetBaseValue(_enemyStat.coolTimeStat, data.coolTime);
            _statCompo.SetBaseValue(_enemyStat.scoreStat, data.score);
            _statCompo.SetBaseValue(_enemyStat.expStat, data.exp);
        }

        #region Data section

        public int Health => (int)_statCompo.GetStat(_enemyStat.healthStat).Value;
        public float Speed => _statCompo.GetStat(_enemyStat.speedStat).Value;
        public int Damage => (int)_statCompo.GetStat(_enemyStat.damageStat).Value;
        public float CoolTime => _statCompo.GetStat(_enemyStat.coolTimeStat).Value;
        public int Score => (int)_statCompo.GetStat(_enemyStat.scoreStat).Value;
        public int Exp => (int)_statCompo.GetStat(_enemyStat.expStat).Value;

        #endregion
    }

    [Serializable]
    public struct EnemyStat
    {
        public StatSO healthStat;
        public StatSO speedStat;
        public StatSO damageStat;
        public StatSO coolTimeStat;
        public StatSO scoreStat;
        public StatSO expStat;
    }
}