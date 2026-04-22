using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;
using Work.JW.Code.Entities;
using Work.JW.Code.Entities.Attacks;

namespace Work.Jiwon.Code.Player
{
    public class PlayerAttackCompo : EntityAttackCompo
    {
        public UnityEvent OnAttackEvent;
        
        [Header("AttackSetting")]
        [SerializeField] private float radius = 2.5f;
        [SerializeField] private int maxReflectionCount = 10;
        [SerializeField] private float comboWindow;
        
        [SerializeField] private GameEventChannelSO skillChannel;

        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO splitBullet;
        public bool IsSplitBullet { get; set; }
        
        private string _bulletBlockModeName;
        private bool _isBlockModeActive;
        
        private EntityAnimator _animator;
        private RaycastHit[] _results;

        private float _lastAttackTime;
        private int _attackCombo;

        private readonly int _attackComboParmHash = Animator.StringToHash("ATTACKCCOMBO");

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _results = new RaycastHit[maxReflectionCount];
            _animator = entity.GetCompo<EntityAnimator>(true);
            _attackCombo = 0;
            
            skillChannel.AddListener<BulletBlockModeEvent>(HandleBlockMode);
        }

        private void OnDisable()
        {
            skillChannel.RemoveListener<BulletBlockModeEvent>(HandleBlockMode);
        }

        public override void Attack()
        {
            OnAttackEvent?.Invoke();
            bool comboCounterOver = _attackCombo > 1;
            bool comboWindowExhaust = Time.time >= _lastAttackTime + comboWindow;
            if (comboWindowExhaust || comboCounterOver)
                _attackCombo = 0;
            
            _animator.SetParam(_attackComboParmHash, _attackCombo);
        }
        
        public override void EndAttack()
        {
            _attackCombo++;
            _lastAttackTime = Time.time;
        }

        public void Reflect()
        {
            Vector3 center = transform.position;
            center.z -= radius;

            int cnt = Physics.SphereCastNonAlloc(center, radius,Vector3.forward , 
                _results, radius * 2, targetLayer.value);
            
            for (int i = 0; i < cnt; i++)
            {
                if (_results[i].collider.TryGetComponent(out Bullet bullet))
                {
                    OnAttackHit?.Invoke();
                    bullet.BlockBullet(_bulletBlockModeName, _isBlockModeActive);
                    Vector3 direction = bullet.GetDirection();
                    
                    bullet.ReflectVec(_results[0].point, _results[0].normal);
                    bullet.SetSpeed(bullet.GetSpeed() + 5);

                    if (IsSplitBullet)
                    {
                        float speed = bullet.GetSpeed();
                        float damage = bullet.GetDamage();
                        Bullet splitBullet = poolManager.Pop(this.splitBullet) as Bullet;
                        splitBullet.transform.position = bullet.transform.position;
                        splitBullet.BlockBullet(_bulletBlockModeName, false);
                        splitBullet.SetDamage((int)damage);
                        splitBullet.SetSpeed(speed);
                        splitBullet.SetDirection(-direction);
                    }
                }
            }
        }
        
        private void HandleBlockMode(BulletBlockModeEvent evt)
        {
            _isBlockModeActive = evt.isModeActive;
            _bulletBlockModeName = evt.skillName;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.white;
        }

#endif
    }
}