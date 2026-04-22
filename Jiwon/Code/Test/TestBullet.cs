using System;
using UnityEngine;

namespace Work.Jiwon.Code.Test
{
    public class TestBullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        
        private Rigidbody _rigid;
        private float _timer;
        
        public Vector3 Direction { get; private set; }
        
        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        public void Fire(Vector3 direction)
        {
            Direction = direction;
            _rigid.linearVelocity = Direction * speed;
        }

        public void SetDirection(Vector3 direction)
        {
            Direction = direction;
            _rigid.linearVelocity = Direction * speed;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}