using System;
using System.Collections.Generic;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.FSM
{
    public class EntityStateMachine
    {
        public EntityState CurrentState { get; private set; }
        private Dictionary<string, EntityState> _states;

        public EntityStateMachine(Entity entity, List<EntityStateDataSO> stateDatas)
        {
            _states = new Dictionary<string, EntityState>();
            foreach (var data in stateDatas)
            {
                Type stateType = Type.GetType(data.className);
                Debug.Assert(stateType != null, $"{data.className} not found");
                
                EntityState state = Activator.CreateInstance(stateType, entity, data.animHash) as EntityState;
                _states.Add(data.stateName, state);
            }
        }

        public void ChangeState(string newStateName, bool forced = false)
        {
            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != null, $"{newStateName} not found");
            if (forced == false && CurrentState == newState)
                return;
            
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void StateMachineUpdate()
        {
            CurrentState?.Update();
        }
    }
}