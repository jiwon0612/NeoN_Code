using System;
using System.Collections;
using System.Collections.Generic;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Serialization;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Attacks;
using Work.JW.Code.FSM;

namespace Work.JW.Code.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
        [field: SerializeField] public GameEventChannelSO LevelChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO GameChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO ScoreChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO EnemyChannel { get; private set; }
        
        protected EnemyDataCompo _dataCompo;
        
        [SerializeField] private List<EntityStateDataSO> states;
        private EntityStateMachine _stateMachine;
        protected Coroutine _shootCoroutine;
        
        [SerializeField] protected float minCoolTime = 2f;
        
        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;

        protected bool _isStop;

        protected override void Awake()
        {
            base.Awake();
            
            _stateMachine = new EntityStateMachine(this, states);
            
            _dataCompo = GetCompo<EnemyDataCompo>();
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void ResetItem()
        {
            IsDead = false;
            GameChannel.AddListener<GameStopEvent>(HandleGameStopEvent);
            
            InitStartData();
            ChangeState("IDLE");
            StartAttack();
            
        }

        protected virtual void Update()
        {
            if (_isStop) return;
            
            _stateMachine?.StateMachineUpdate();
        }

        public void ChangeState(string stateName)
        {
            _stateMachine.ChangeState(stateName);
        }

        public virtual void InitStartData()
        {
            GetCompo<EntityMover>().SetSpeed(_dataCompo.Speed);
        }

        protected abstract IEnumerator ShootCoroutine();
        
        public void StartAttack()
        {
            if(_shootCoroutine != null)
                StopCoroutine(_shootCoroutine);
            
            _shootCoroutine = StartCoroutine(ShootCoroutine());
        }
        
        private void OnDisable()
        {
            StopAttack();
        }

        private void StopAttack()
        {
            if(_shootCoroutine != null)
                StopCoroutine(_shootCoroutine);
        }

        public override void OnDead()
        {
            EnemyChannel.RaiseEvent(EnemyEvents.EnemyDeadEvent);
            
            //Score 추가
            int score = _dataCompo.Score;
            
            var scoreEvent = ScoreEvents.AddScoreEvent.Initializer(score);
            ScoreChannel.RaiseEvent(scoreEvent);
            
            //Exp 추가
            float exp = _dataCompo.Exp;
            
            var expEvent = LevelEvents.AddEXPEvent.Initializer(exp);
            LevelChannel.RaiseEvent(expEvent);
            
            GameChannel.RemoveListener<GameStopEvent>(HandleGameStopEvent);
            
            base.OnDead();
            _myPool.Push(this);

        }
        
        protected virtual void HandleGameStopEvent(GameStopEvent evt)
        {
            _isStop = evt.isStop;
            GetCompo<EntityMover>().StopMovement(evt.isStop);

            if (evt.isStop)
            {
                StopAttack();
            }
            else
            {
                StartAttack();
            }
        }
    }
}