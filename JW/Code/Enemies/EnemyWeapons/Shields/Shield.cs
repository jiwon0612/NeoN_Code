using System;
using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Enemies.EnemyWeapons.Shields
{
    public class Shield : MonoBehaviour, IPoolable
    {
        private Transform _targetTrm;
        private float _radius = 2f; //반지름
        private float _moveSpeed = 2f; //속도
        private float _startAngle; //시작 각도

        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        
        public void Init(Transform target, float radius, float moveSpeed, float startAngle)
        {
            _targetTrm = target;
            _radius = radius;
            _moveSpeed = moveSpeed;
            _startAngle = startAngle;
        }
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            
        }
        
        private void Update()
        {
            if(_targetTrm == null) return;

            SetShieldPosition(_targetTrm);
            SetShieldRotation(_targetTrm);
        }
        
        private void SetShieldPosition(Transform target)
        {
            float rad = _startAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad + Time.time * _moveSpeed) * _radius;
            float z = Mathf.Sin(rad + Time.time * _moveSpeed) * _radius;
            
            Vector3 offset = new Vector3(x, 0, z);
            transform.position = target.position + offset;
        }

        private void SetShieldRotation(Transform target)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookAt = Quaternion.LookRotation(-dir);
            float t = 8 * _moveSpeed * Time.deltaTime;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, t);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IAttackable attackable) &&
                other.gameObject.layer == LayerMask.NameToLayer("AttackToEnemy"))
            {
                attackable.Attack();
                OnDead();
            }
        }

        public void OnDead()
        {
            _myPool.Push(this);
        }
    }
}