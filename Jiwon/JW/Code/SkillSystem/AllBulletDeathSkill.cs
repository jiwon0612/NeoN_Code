using UnityEngine;
using Work.Jiwon.Code.Combts;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;
using Work.JW.Code.Entities;

namespace Work.JW.Code.SkillSystem
{
    public class AllBulletDeathSkill : ActiveSkill
    {
        [SerializeField] private PoolTypeSO effectPoolType;
        [SerializeField] private PoolManagerSO poolManager;
        
        public override void Initialize(Entity entity)
        {
            
        }

        public override void UseSkill()
        {
            Bullet[] bullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);
            
            foreach (var bullet in bullets)
            {
                if (bullet)
                {
                    VFXPlayer vfx = poolManager.Pop(effectPoolType) as VFXPlayer;
                    vfx.PlayVFX(bullet.transform.position, Quaternion.identity);
                    bullet.OnDead();
                }
            }
        }
    }
}