using System;
using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Enemies.EnemyWeapons.Shields
{
    public class ShieldManager : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private PoolManagerSO pool;
        [SerializeField] private PoolTypeSO shieldPoolType;
        private List<Shield> _shieldList;
        [SerializeField] private int wantSpawnCount = 3;
        [SerializeField] private float radius = 2f;
        [SerializeField] private float moveSpeed = 2f;
        private float _rotationOffset;
        
        public void Initialize(Entity entity)
        {
            _shieldList = new List<Shield>();
            
            _rotationOffset = 360f / wantSpawnCount;
        }

        public void InitShield(Transform targetTrm)
        {
            float currentRotationOffset = _rotationOffset;
            
            foreach (var item in _shieldList)
            {
                item.Init(targetTrm, radius, moveSpeed,currentRotationOffset);
                
                currentRotationOffset += _rotationOffset;
            }
        }
        
        public void SpawnShield()
        {
            for (int i = 0; i < wantSpawnCount; i++)
            {
                var item = pool.Pop(shieldPoolType) as Shield;
                _shieldList.Add(item);
            }
        }

        public void OnDead()
        {
            foreach (var item in _shieldList)
            {
                item.OnDead();
            }
        }
    }
}