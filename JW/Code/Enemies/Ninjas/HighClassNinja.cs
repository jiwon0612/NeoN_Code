using System.Collections;
using UnityEngine;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class HighClassNinja : Enemy
    {
        [SerializeField] protected float attackPower;
        protected EnemyAttackCompo _attackCompo;
        protected Transform _target;
        
        protected override void Awake()
        {
            base.Awake();
            _attackCompo = GetCompo<EnemyAttackCompo>();
        }
        
        public override void InitStartData()
        {
            _target = _dataCompo.TargetTrm;
            
            _attackCompo.SetAttackDamage(_dataCompo.Damage);
            _attackCompo.SetAttackPower(attackPower);
        }

        protected override void Update()
        {
            base.Update();
            _attackCompo.LookAtTarget(_target.position);
        }
        
        protected override IEnumerator ShootCoroutine()
        {
            yield return null;
        }
    }
}