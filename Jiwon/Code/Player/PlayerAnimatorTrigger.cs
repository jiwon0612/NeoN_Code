using System;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Player
{
    public class PlayerAnimatorTrigger : EntityAnimatorTrigger
    {
        public event Action OnAttackTrigger;
        public event Action OnSkillTrigger; 

        private void AttackTrigger() => OnAttackTrigger?.Invoke();
        
        private void SkillTrigger() => OnSkillTrigger?.Invoke();
    }
}