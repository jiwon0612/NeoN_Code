using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Jiwon.Code.StatSystem;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Entities
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInit, IDamageable
    {
        [SerializeField] private StatSO healthStat;

        public UnityEvent<float, float> OnHealthChangedEvent;
        public UnityEvent OnDeathEvent;

        private int _currentMaxHealth;
        private int _currentHealth;
        private EntityStat _statCompo;
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInitialize()
        {
            _statCompo = _entity.GetCompo<EntityStat>();
            _statCompo.GetStat(healthStat).OnValueChange += HandleHealthChange;

            _currentMaxHealth = (int)_statCompo.GetStat(healthStat).Value;
            _currentHealth = _currentMaxHealth;
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(healthStat).OnValueChange -= HandleHealthChange;
        }
        
        public int GetCurrentMaxHealth() => _currentMaxHealth;

        private void HandleHealthChange(StatSO stat, float current, float previous)
        {
            _currentMaxHealth = (int)current;
            _currentHealth += (int)current - (int)previous;
            OnHealthChangedEvent?.Invoke(_currentHealth, _currentMaxHealth);
        }

        public void ApplyDamage(int damage)
        {
            if (_entity.IsDead)
                return;

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                OnDeathEvent?.Invoke();
                _entity.OnDead();
            }

            OnHealthChangedEvent?.Invoke(_currentHealth, _currentMaxHealth);
        }
    }
}