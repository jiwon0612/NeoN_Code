using Work.Jiwon.Code.Entities;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem
{
    public abstract class PassiveSkill : Skill
    {
        public bool IsActive { get; set; }

        public override void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public virtual void ActiveSkill() => IsActive = true;
        public virtual void DisableSkill() => IsActive = false;
    }
}