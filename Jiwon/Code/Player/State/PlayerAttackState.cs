using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Player.State
{
    public class PlayerAttackState : PlayerState
    {
        private readonly PlayerAttackCompo _attackCompo;
        private readonly PlayerAnimatorTrigger _animatorTrigger;
        
        public PlayerAttackState(Entity entity, int animHash) : base(entity, animHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
            _animatorTrigger = entity.GetCompo<PlayerAnimatorTrigger>();
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAttackTrigger += _attackCompo.Reflect;
            _attackCompo.Attack();
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackTrigger -= _attackCompo.Reflect;
            _attackCompo.EndAttack();
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.StateChange("IDLE");
        }
    }
}