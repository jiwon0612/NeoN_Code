using System;
using UnityEngine;
using Work.Jiwon.Code.Combts;
using Work.Jiwon.Code.Entities;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;

namespace Work.Jiwon.Code.SkillSystem.Skills.ShurikenSkills
{
    public class ShurikenBullet : Bullet
    {
        private bool _isEnable;

        protected override void FixedUpdate() =>
            _rigid.linearVelocity = Direction * _moveSpeed;

        private void OnEnable()
        {
            _isEnable = true;
            _timer = 0;
        }

        private void Update()
        {
            if (_isEnable)
            {
                if (_timer >= lifeTime)
                {
                    OnDead();
                }

                _timer += Time.deltaTime;
            }
        }

        private void OnDisable()
        {
            _isEnable = false;
            _timer = 0;
        }

        protected override void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable enemy))
            {
                ParticlePlayer vfx = poolManager.Pop(hitEffect) as ParticlePlayer;
                vfx.PlayParticle(other.transform.position + (Vector3.up * 1.5f), Quaternion.identity);
                enemy.ApplyDamage(_damage);
            }
        }
    }
}