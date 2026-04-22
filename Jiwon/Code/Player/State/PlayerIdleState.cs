using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Player.State
{
    public class PlayerIdleState : PlayerState
    {
        protected PlayerInputSO _input;
        
        public PlayerIdleState(Entity entity, int animHash) : base(entity, animHash)
        {
            _input = _player.PlayerInput;
        }

        public override void Enter()
        {
            base.Enter();
            _input.OnAttackPressed += HandleAttackChange;
            _input.OnUseSkillPressed += HandleUseSkillChange;
        }

        public override void Exit()
        {
            _input.OnAttackPressed -= HandleAttackChange;
            _input.OnUseSkillPressed -= HandleUseSkillChange;
            base.Exit();
        }

        private void HandleUseSkillChange()
        {
            _player.StateChange("USESKILL");
        }

        private void HandleAttackChange()
        {   
            _player.StateChange("ATTACK");
        }
    }
}