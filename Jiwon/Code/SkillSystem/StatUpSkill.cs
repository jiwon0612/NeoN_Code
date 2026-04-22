using System;
using UnityEngine;
using Work.Jiwon.Code.Entities;
using Work.Jiwon.Code.StatSystem;
using Work.JW.Code.Entities;

[Serializable]
public struct StatModifyValue
{
    public StatSO stat;
    public float modifyValue;
}

namespace Work.Jiwon.Code.SkillSystem
{
    public class StatUpSkill : PassiveSkill
    {
        public StatModifyValue[] stats;
        public int maxOverlappingCount;
        private int currentStatCount;
        private EntityStat _statCompo;

        public int CurrentStatCount
        {
            get => currentStatCount;
            set
            {
                if (value > maxOverlappingCount)
                    currentStatCount = maxOverlappingCount;
                else
                    currentStatCount = value;
            }
        }

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public override void ActiveSkill()
        {
            base.ActiveSkill();

            AddModifier(_statCompo);
        }

        public override void DisableSkill()
        {
            base.DisableSkill();
            
            RemoveModifier(_statCompo);
        }

        public void AddModifier(EntityStat statCompo)
        {
            CurrentStatCount++; 
            Debug.Log(currentStatCount);
            foreach (var stat in stats)
            {
                StatSO targetStat = statCompo.GetStat(stat.stat);
                if (targetStat != null)
                    targetStat.AddModifier(this, stat.modifyValue * CurrentStatCount);
            }
        }

        public void RemoveModifier(EntityStat statCompo)
        {
            CurrentStatCount = 0;
            foreach (var stat in stats)
            {
                StatSO targetStat = statCompo.GetStat(stat.stat);
                if (targetStat != null)
                    targetStat.RemoveModifier(this);
            }
        }
    }
}