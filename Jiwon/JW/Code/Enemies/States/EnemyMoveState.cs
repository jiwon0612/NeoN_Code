using UnityEngine;
using UnityEngine.AI;
using Work.JW.Code.Entities;
using Work.JW.Code.FSM;

namespace Work.JW.Code.Enemies.States
{
    public class EnemyMoveState : EntityState
    {
        private Enemy _enemy;
        EntityMover _mover;

        Vector3 _movePosition;
        private float _xMaxPosition;
        private float _zMaxPosition;
        
        public EnemyMoveState(Entity entity, int animHash) : base(entity, animHash)
        {
            _enemy = entity as Enemy;
            _mover = entity.GetCompo<EntityMover>();

            _xMaxPosition = 5f;
            _zMaxPosition = 5f;
        }

        public override void Enter()
        {
            base.Enter();
            
            _movePosition = AddMovePositionVec(_xMaxPosition, _zMaxPosition);

            Vector3 movePos = _enemy.transform.position + _movePosition;
            _mover.SetMovement(movePos);
        }

        public override void Update()
        {
            if (_mover.IsMoveStopped())
            {
                _enemy.ChangeState("IDLE");
            }
        }

        private Vector3 AddMovePositionVec(float maxXPos, float maxZPos)
        {
            float xPos = Random.Range(-maxXPos, maxXPos);
            float zPos = Random.Range(-maxZPos, maxZPos);
            
            Vector3 point = new Vector3(xPos, 0, zPos);
            return point;
        }
    }
}