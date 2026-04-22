using System;
using UnityEngine;
using Work.Jiwon.Code.Entities;
using Work.Jiwon.Code.Player;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.CloneSkills
{
    public class Clone : Entity, IPoolable, IDamageable
    {
        #region PoolRegion

        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _pool;

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
        }

        #endregion
        
        public bool IsEnabled { get; private set; }
        
        private EntityAnimator _animator;
        private PlayerAttackCompo _attackCompo;
        private EntityMover _mover;
        private PlayerInputSO _input;
        [SerializeField] private EntityFinderSO playerFinder;
        private Player.Player _player;

        private Vector3 _offset;
        
        private readonly int _attachHash = Animator.StringToHash("ATTACK");

        protected override void InitComponents()
        {
            base.InitComponents();
            _attackCompo = GetCompo<PlayerAttackCompo>(true);
            _animator = GetCompo<EntityAnimator>(true);
            _mover = GetCompo<EntityMover>(true);
            
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
            _player = playerFinder.target as Player.Player;
        }

        public void InitClone(Vector3 offset)
        {
            _offset = offset;
            
            _player.GetCompo<PlayerAnimatorTrigger>().OnAnimationEndTrigger += HandleAttackEndTrigger;
            _player.GetCompo<PlayerAnimatorTrigger>().OnAttackTrigger += HandleAttackTriggerEvent;
            _player.PlayerInput.OnAttackPressed += HandleAttackEvent;
            _input = _player.PlayerInput;
            IsEnabled = true;
        }

        private void OnDestroy()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackEvent;
            _player.GetCompo<PlayerAnimatorTrigger>().OnAnimationEndTrigger -= HandleAttackEndTrigger;
            _player.GetCompo<PlayerAnimatorTrigger>().OnAttackTrigger -= HandleAttackTriggerEvent;
        }

        private void HandleAttackTriggerEvent()
        {
            if (IsEnabled)
               _attackCompo.Reflect();
        }

        private void HandleAttackEvent()
        {
            _animator.SetParam(_attachHash, true);
            _attackCompo.Attack();
        }

        private void HandleAttackEndTrigger()
        {
            _animator.SetParam(_attachHash, false);
            _attackCompo.EndAttack();
        }

        private void Update()
        {
            if (IsEnabled && !_player.IsDead)
                _mover.WarpTo(_input.GetWoldPosition() + _offset);
        }

        private void OnDead()
        {
            IsEnabled = false;
            _pool.Push(this);
        }

        public void ApplyDamage(int damage)
        {
            if (IsEnabled)
                OnDead();
        }
    }
}