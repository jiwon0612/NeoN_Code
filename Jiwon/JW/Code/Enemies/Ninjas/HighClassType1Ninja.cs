using System.Collections;
using UnityEngine;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class HighClassType1Ninja : HighClassNinja
    {
        protected override IEnumerator ShootCoroutine()
        {
            int cnt = 0;
            float rand = Random.Range(minCoolTime, _dataCompo.CoolTime);
            yield return new WaitForSeconds(rand);
            
            while(cnt++ < 3)
            {
                _attackCompo.SetDirection(transform.forward);
                _attackCompo.Attack();
                
                yield return new WaitForSeconds(0.2f);
            }
            
            StartAttack();
        }
    }
}