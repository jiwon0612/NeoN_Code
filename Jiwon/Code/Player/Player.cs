using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Jiwon.Code.Entities;
using Work.Jiwon.Code.EventSystem;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Core.Dependencies;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;
using Work.JW.Code.FSM;

namespace Work.Jiwon.Code.Player
{
    public class Player : Entity, IDependencyProvider
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get;private set; }
        [SerializeField] private List<EntityStateDataSO> _states;
        [SerializeField] private GameEventChannelSO gameChannel;
        [SerializeField] private GameEventChannelSO playerChannel;

        [Provide]
        public Player ProvidePlayer() => this;
        
        private EntityHealth _health;
        private EntityStateMachine _stateMachine;
        private SkillCompo _skillCompo;

        public bool IsStop { get; private set; }

        protected override void AddComponents()
        {
            base.AddComponents();
            _stateMachine = new EntityStateMachine(this,_states);
            _skillCompo = GetCompo<SkillCompo>();
            _health = GetCompo<EntityHealth>();
            
            gameChannel.AddListener<GameStopEvent>(HandleGameStopEvent);
            _health.OnDeathEvent.AddListener(HandleDeathEvent);
            _health.OnHealthChangedEvent.AddListener(HandleHitEvent);
            
            IsDead = false;
            IsStop = false;
        }

        private void HandleHitEvent(float health, float maxHealth)
        {
            playerChannel.RaiseEvent(PlayerEvents.PlayerHitEvent.Initialize(health, maxHealth));
        }

        private void HandleDeathEvent()
        {
            IsStop = true;
            playerChannel.RaiseEvent(PlayerEvents.PlayerDeadEvent);
        }

        private void OnDestroy()
        {
            gameChannel.RemoveListener<GameStopEvent>(HandleGameStopEvent);
        }

        private void HandleGameStopEvent(GameStopEvent evt)
        {
            IsStop = evt.isStop;
        }

        private void Start()
        {
            StateChange("IDLE");
            int healthValue = GetCompo<EntityHealth>().GetCurrentMaxHealth();
            playerChannel.RaiseEvent(PlayerEvents.PlayerHitEvent.Initialize(healthValue, healthValue));
        }

        private void Update()
        {
            _stateMachine.StateMachineUpdate();
        }
        
        public void StateChange(string newState, bool forced = false) => _stateMachine.ChangeState(newState, forced);
    }
}