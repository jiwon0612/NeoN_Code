using System;
using UnityEngine;

namespace Work.Jiwon.Code.Test
{
    public class TestShooter : MonoBehaviour
    {
        [SerializeField] private TestBullet bulletPrefab;
        [SerializeField] private float shootDelay;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform target;

        private float _timer;
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= shootDelay)
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            _timer = 0;
            TestBullet bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shootPoint.position;
            
            Vector3 dir = target.position - shootPoint.transform.position;
            bullet.Fire(dir.normalized);
        }
    }
}