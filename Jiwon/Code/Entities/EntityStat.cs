using System.Linq;
using UnityEngine;
using Work.Jiwon.Code.StatSystem;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Entities
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private StatSO[] _stats;
        
        public Entity Owner { get;private set; }
        
        public void Initialize(Entity entity)
        {
            Owner = entity;

            _stats = statOverrides.Select(stat => stat.CreateStat()).ToArray();
        }

        public StatSO GetStat(StatSO targetStat)
        {
            Debug.Assert(targetStat != null, $"{targetStat.statName} is null");
            
            return _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
        }

        public bool TryGetStat(StatSO targetStat, out StatSO stat)
        {
            Debug.Assert(targetStat != null, "Target stat is null");
            
            stat = _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
            return stat;
        }
        
        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value) => GetStat(stat).AddModifier(key, value);
        public void RemoveModifier(StatSO stat, object key) => GetStat(stat).RemoveModifier(key);

        public void CleanAllModifiers()
        {
            foreach (var item in _stats)
            {
                item.ClearAllModifier();
            }
        }
    }
}