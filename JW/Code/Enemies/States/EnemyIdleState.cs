using System;
using System.Collections;
using UnityEngine;
using Work.JW.Code.Entities;
using Work.JW.Code.FSM;
using Random = UnityEngine.Random;

namespace Work.JW.Code.Enemies.States
{
    public class EnemyIdleState : EntityState
    {
        private Enemy _enemy;
        private WaitForSeconds waitTime;
        private int _stateMaxTypeNumber;
        private Coroutine _stateSelectCoroutine;
        
        public EnemyIdleState(Entity entity, int animHash) : base(entity, animHash)
        {
            _enemy = entity as Enemy;
            _stateMaxTypeNumber = Enum.GetValues(typeof(EnemyStateType)).Length;
        }
        
        public override void Enter()
        {
            base.Enter();
            _stateSelectCoroutine = _enemy.StartCoroutine(StateSelectCoroutine());
        }

        private IEnumerator StateSelectCoroutine()
        {
            EnemyStateType type = (EnemyStateType)Random.Range(0, _stateMaxTypeNumber);
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            switch (type)
            {
                case EnemyStateType.IDLE:
                    StopCoro(_stateSelectCoroutine);
                    _stateSelectCoroutine = _enemy.StartCoroutine(StateSelectCoroutine());
                    break;
                case EnemyStateType.MOVE:
                    _enemy.ChangeState("MOVE");
                    break;
            }
        }

        private void StopCoro(Coroutine coro)
        {
            if(coro != null)
                _enemy.StopCoroutine(coro);
        }

        public override void Exit()
        {
            StopCoro(_stateSelectCoroutine);
            base.Exit();
        }
    }
}