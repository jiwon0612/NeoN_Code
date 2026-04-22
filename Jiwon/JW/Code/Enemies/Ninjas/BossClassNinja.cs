using System.Collections;
using Ami.Extension;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;
using Random = UnityEngine.Random;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class BossClassNinja : Enemy
    {
        [SerializeField] private float attackPower;
        [SerializeField] private Vector3 lookForward;

        [Header("Pattern section")] [SerializeField]
        private float offsetDirection;

        [SerializeField] private int shootBulletCnt;

        private EnemyAttackCompo _attackCompo;
        private Transform _target;

        private Coroutine _pattern1Coroutine;
        private Coroutine _pattern2Coroutine;
        private Coroutine _pattern3Coroutine;

        protected override void Awake()
        {
            base.Awake();
            _attackCompo = GetCompo<EnemyAttackCompo>();
        }

        public override void InitStartData()
        {
            _target = _dataCompo.TargetTrm;

            var evt = GameEvents.BossSpawnEvent.Initializer(this);
            GameChannel.RaiseEvent(evt);

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
            float rand = Random.Range(minCoolTime, _dataCompo.CoolTime);
            yield return new WaitForSeconds(rand);

            var patternType = (BossPatternType)Random.Range(0, (int)BossPatternType.END);

            switch (patternType)
            {
                case BossPatternType.Type1:
                    _pattern1Coroutine = StartCoroutine(ShootPattern1());
                    break;
                case BossPatternType.Type2:
                    _pattern2Coroutine = StartCoroutine(ShootPattern2());
                    break;
                case BossPatternType.Type3:
                    _pattern3Coroutine = StartCoroutine(ShootPattern3());
                    break;
            }

            StartAttack();
        }

        protected IEnumerator ShootPattern1()
        {
            //기본 표창 난사
            int cnt = 0;

            _attackCompo.SetBulletType(BulletType.DEFAULT);

            while (cnt++ < shootBulletCnt * 2)
            {
                yield return new WaitWhile(() => _isStop);

                _attackCompo.SetDirection(transform.forward);
                _attackCompo.Attack();

                yield return new WaitForSeconds(0.15f);
            }
        }

        protected IEnumerator ShootPattern2()
        {
            int dirCnt = 0;
            int shootCnt = 0;
            int whatCnt = 5;

            float direction = -(offsetDirection * (whatCnt / 2));

            _attackCompo.SetBulletType(BulletType.DEFAULT);

            while (shootCnt++ < shootBulletCnt)
            {
                while (dirCnt++ < whatCnt)
                {
                    yield return new WaitWhile(() => _isStop);

                    Vector3 dir = Quaternion.Euler(0, direction, 0) * transform.forward;

                    _attackCompo.SetDirection(dir);
                    _attackCompo.Attack();

                    direction += offsetDirection;
                    yield return null;
                }

                dirCnt = 0;
                direction = -(offsetDirection * (whatCnt / 2));

                yield return new WaitForSeconds(0.2f);
            }
        }

        protected IEnumerator ShootPattern3()
        {
            //대형 표창 난사
            int cnt = 0;
            _attackCompo.SetBulletType(BulletType.LARGE);

            while (cnt++ < shootBulletCnt)
            {
                yield return new WaitWhile(() => _isStop);

                _attackCompo.SetDirection(transform.forward);
                _attackCompo.Attack();

                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public enum BossPatternType
    {
        Type1,
        Type2,
        Type3,
        END
    }
}