using System.Collections;
using UnityEngine;
using Work.JW.Code.Entities.Attacks;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class LowClassNinja : Enemy
    {
        [SerializeField] private float attackPower;
        [SerializeField] private Vector3 lookForward;
        private EnemyAttackCompo _attackCompo;

        protected override void Awake()
        {
            base.Awake();
            _attackCompo = GetCompo<EnemyAttackCompo>();
        }

        public override void InitStartData()
        {
            _attackCompo.LookAtTarget(transform.position + lookForward, false);
            
            _attackCompo.SetAttackDamage(_dataCompo.Damage);
            _attackCompo.SetAttackPower(attackPower);
        }

        protected override IEnumerator ShootCoroutine()
        {
            float rand = Random.Range(minCoolTime, _dataCompo.CoolTime);

            yield return new WaitForSeconds(rand);
            
            _attackCompo.SetDirection(transform.forward);
            _attackCompo.Attack();

            StartAttack();
        }
    }
}