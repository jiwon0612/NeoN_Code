using System.Collections;
using UnityEngine;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class HighClassType3Ninja : HighClassNinja
    {
        public override void InitStartData()
        {
            base.InitStartData();
            _attackCompo.SetBulletType(BulletType.LARGE);
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