using System;
using UnityEngine;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.ShurikenSkills
{
    public class ShurikenSkill : ActiveSkill
    {
        [SerializeField] private Transform fireMuzzle;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO poolType;
        [SerializeField] private float skillSpeed;
        [SerializeField] private int skillDamage;

        public override void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public override void UseSkill()
        { 
            ShurikenBullet bullet = poolManager.Pop(poolType) as ShurikenBullet;
            bullet.SetDirection(_entity.transform.forward);
            bullet.transform.position = fireMuzzle.position;
            
            bullet.SetSpeed(skillSpeed);
            bullet.SetDamage(skillDamage);
        }
    }
}