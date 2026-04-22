using UnityEngine;
using Work.Jiwon.Code.Player;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.SplitBulletSkills
{
    public class SplitBulletSkill : PassiveSkill
    {
        private PlayerAttackCompo _attackCompo;
        
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _attackCompo = _entity.GetCompo<PlayerAttackCompo>();
            Debug.Assert(_attackCompo != null, $"is Not Player");
        }

        public override void ActiveSkill()
        {
            base.ActiveSkill();
            _attackCompo.IsSplitBullet = true;
        }

        public override void DisableSkill()
        {
            _attackCompo.IsSplitBullet = false;
            base.DisableSkill();
        }
    }
}