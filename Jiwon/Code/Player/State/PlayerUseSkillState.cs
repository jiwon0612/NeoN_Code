using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Player.State
{
    public class PlayerUseSkillState : PlayerState
    {
        private readonly PlayerAnimatorTrigger _animatorTrigger;
        private readonly SkillCompo _skillCompo;
        
        public PlayerUseSkillState(Entity entity, int animHash) : base(entity, animHash)
        {
            _animatorTrigger = entity.GetCompo<PlayerAnimatorTrigger>();
            _skillCompo = entity.GetCompo<SkillCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAnimationEndTrigger += HandleEndEvent;
            bool isSuccess = _skillCompo.UseActiveSkill();
            
            if (!isSuccess)
                _player.StateChange("IDLE");
        }

        public override void Exit()
        {
            _animatorTrigger.OnAnimationEndTrigger -= HandleEndEvent;
            base.Exit();
        }

        private void HandleEndEvent()
        {
            _isTriggerCall = true;
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.StateChange("IDLE");
        }
    }
}