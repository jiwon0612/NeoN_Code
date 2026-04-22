using UnityEngine;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Attacks;

namespace Work.JW.Code.Enemies
{
    public class EnemyAttackCompo : EntityAttackCompo
    {
        [SerializeField] private PoolManagerSO poolM;
        [SerializeField] private PoolTypeSO[] bulletPoolTypes;
        
        private PoolTypeSO _curBulletPoolItem;
        private WaitForSeconds _waitShoot;

        [SerializeField] private float rotateSpeed = 8;
        [SerializeField] private Transform muzzlePos;

        private Vector3 _shootDir;
        private float _attackPower;
        private int _attackDamage;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _curBulletPoolItem = bulletPoolTypes[0];
        }

        public override void Attack()
        {
            var bullet = poolM.Pop(_curBulletPoolItem) as Bullet;

            bullet.transform.position = muzzlePos.position;
            bullet.SetOwner(_entity);
            bullet.SetSpeed(_attackPower);
            bullet.SetDirection(_shootDir);
            bullet.SetDamage(_attackDamage);
        }

        public void SetDirection(Vector3 direction) => _shootDir = direction;
        public void SetAttackPower(float speed) => _attackPower = speed;
        public void SetAttackDamage(int damage) => _attackDamage = damage;
        public void SetBulletType(BulletType type) => _curBulletPoolItem = bulletPoolTypes[(int) type];

        public override void EndAttack()
        {
        }

        public void LookAtTarget(Vector3 targetPos, bool isImmediately = true)
        {
            Vector3 moveDir = targetPos - transform.position;
            Quaternion lookAtTarget = Quaternion.LookRotation(moveDir);

            if (isImmediately)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, lookAtTarget, Time.deltaTime * rotateSpeed);
            }
            else
            {
                _entity.transform.rotation = lookAtTarget;
            }
        }
    }

    public enum BulletType
    {
        DEFAULT,
        LARGE
    }
}