using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Work.Jiwon.Code.Combts;
using Work.Jiwon.Code.Entities;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Enemies.EnemyWeapons.Bullets
{
    public class Bullet : MonoBehaviour, IPoolable, IAttackable
    {
        public UnityEvent OnDeathEvent;

        [SerializeField] protected PoolTypeSO hitEffect;
        [SerializeField] protected PoolManagerSO poolManager;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected float lifeTime;

        [SerializeField] protected string attackToPlayerLayerName;
        [SerializeField] protected string attackToEnemyLayerName;
        [SerializeField] protected GameEventChannelSO gameChannel;

        public static float speedMultiplier = 1f;

        protected Entity _shootOwner;

        protected Rigidbody _rigid;
        [SerializeField] protected BulletRenderer bulletRenderer;

        protected float _timer;
        protected bool _isHit;

        protected int _damage;
        protected float _tempSpeed;

        public Vector3 Direction { get; private set; }

        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        protected Pool _myPool;

        protected IBulletMode _currentBulletMode;

        private Coroutine _lifeTimeCoroutine;

        protected virtual void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _shootOwner = null;
        }

        public virtual void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void ResetItem()
        {
            gameChannel.AddListener<GameStopEvent>(HandleGameStopEvent);
            bulletRenderer.SetColor(false);
            _shootOwner = null;
            
            _lifeTimeCoroutine = StartCoroutine(WaitLifeTimeCoroutine());
        }

        private IEnumerator WaitLifeTimeCoroutine()
        {
            yield return new WaitForSeconds(lifeTime);
            OnDead();
        }

        protected virtual void FixedUpdate()
        {
            _currentBulletMode?.Update();

            transform.position += Direction * (_moveSpeed * speedMultiplier * Time.fixedDeltaTime);
        }

        public virtual void SetDirection(Vector3 direction)
        {
            Direction = new Vector3(direction.x, 0, direction.z).normalized;
        }

        public Vector3 GetDirection() => Direction;
        public float GetSpeed() => _moveSpeed;
        public float GetDamage() => _damage;

        private void SetBulletMode(string bulletModeName, bool isActive)
        {
            _currentBulletMode?.Exit();

            if (!isActive) return;

            IBulletMode mode = GetComponentsInChildren<IBulletMode>().FirstOrDefault(m => m.ModeName == bulletModeName);
            
            mode?.Enter(this);
            _currentBulletMode ??= mode;
        }

        public virtual void SetOwner(Entity shootOwner) => _shootOwner = shootOwner;

        public virtual void SetSpeed(float speed)
        {
            _moveSpeed = speed;
            _tempSpeed = speed;
        }

        public virtual void SetSpeedMultiplier(float speed)
        {
            speedMultiplier = speed;
            SetDirection(Direction);
        }

        public virtual void SetDamage(int damage) => _damage = damage;

        public virtual Entity GetShootOwner() => _shootOwner;

        public virtual void ReflectVec(Vector3 hitPoint, Vector3 normal)
        {
            Vector3 incoming = Direction.normalized;
            Vector3 dir = Vector3.Reflect(incoming, normal).normalized;
            
            SetDirection(dir);

            // 법선벡터
            Debug.DrawRay(hitPoint, normal * 2, Color.green, 2f);
            // 반사벡터
            Debug.DrawRay(hitPoint, dir * 2, Color.red, 2f);
        }

        public virtual void Attack(Entity hitTarget)
        {
            OnDead();
        }

        public virtual void BlockBullet(string bulletModeName, bool isActive)
        {
            // gameObject.layer = (int)Mathf.Log(, 2);
            gameObject.layer = LayerMask.NameToLayer(attackToEnemyLayerName);
            bulletRenderer.SetColor(true);

            SetBulletMode(bulletModeName, isActive);
        }

        public virtual void OnDead()
        {
            OnDeathEvent?.Invoke();

            if(_lifeTimeCoroutine != null)
                StopCoroutine(_lifeTimeCoroutine);
            
            //초기화
            gameChannel.RemoveListener<GameStopEvent>(HandleGameStopEvent);
            _isHit = false;
            SetBulletMode(null, false);
            bulletRenderer.SetColor(false);
            gameObject.layer = LayerMask.NameToLayer(attackToPlayerLayerName);

            _myPool.Push(this);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                if (_isHit)
                    OnDead();
                else
                {
                    _isHit = true;
                    ReflectVec(other.contacts[0].point, other.contacts[0].normal);
                }
            }

            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                ParticlePlayer vfx = poolManager.Pop(hitEffect) as ParticlePlayer;
                vfx.PlayParticle(other.transform.position + (Vector3.up * 1.5f), Quaternion.identity);
                damageable.ApplyDamage(_damage);
                OnDead();
            }
        }

        private void HandleGameStopEvent(GameStopEvent evt)
        {
            if (evt.isStop)
            {
                _tempSpeed = _moveSpeed;
                _moveSpeed = 0;
            }
            else
            {
                _moveSpeed = _tempSpeed;
            }
        }
    }
}