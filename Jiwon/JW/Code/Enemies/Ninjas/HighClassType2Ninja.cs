using System.Collections;
using UnityEngine;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class HighClassType2Ninja : HighClassNinja
    {
        [SerializeField] private float offsetDirection;

        protected override IEnumerator ShootCoroutine()
        {
            int cnt = 0;
            float direction = -offsetDirection;
            float rand = Random.Range(minCoolTime, _dataCompo.CoolTime);
            yield return new WaitForSeconds(rand);
            
            while(cnt++ < 3)
            {
                Vector3 dir = Quaternion.Euler(0, direction, 0) * transform.forward;
                
                _attackCompo.SetDirection(dir);
                _attackCompo.Attack();
                
                direction += offsetDirection;
                yield return null;
            }
            
            StartAttack();
        }
    }
}