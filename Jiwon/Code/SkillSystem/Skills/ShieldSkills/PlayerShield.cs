using System;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using UnityEngine.Events;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.ShieldSkills
{
    public class PlayerShield : MonoBehaviour, IPoolable
    {
        public UnityEvent OnHitEvent;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO shieldRipples;
        [SerializeField] private float speed;

        private readonly int _durationHash = Shader.PropertyToID("Duration");
        
        private VisualEffect _visualEffect;
        private Collider _collider;
        private Rigidbody _rigidbody;

        private float _duration;
        private float _timer;
        private bool _isEnable;
        private Entity _target;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>(); 
            _visualEffect = GetComponent<VisualEffect>();
        }

        public void StartShield(float duration, Entity target)
        {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _visualEffect.SetFloat(_durationHash, duration);
            //_visualEffect.playRate = 0;
            _visualEffect.Play();
            _duration = duration;
            _target = target;
            _isEnable = true;
        }

        private void Update()
        {
            if (_isEnable)
            {
                if (_target != null)
                {
                    Vector3 point = transform.position;
                    Vector3 point2 = _target.transform.position;
                    point.y = transform.position.y;
                    point2.y = transform.position.y;
                    transform.position = Vector3.Lerp(point, point2, speed * Time.deltaTime);
                }
                
                _timer += Time.deltaTime;
                if (_timer >= _duration)
                {
                    _myPool.Push(this);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                OnHitEvent?.Invoke();
                PlayerShieldEffect vEffect = poolManager.Pop(shieldRipples) as PlayerShieldEffect;
                vEffect.transform.position = transform.position;
                vEffect.StartEffect(other.contacts[0].point);
                bullet.OnDead();
            }
        }

        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _timer = 0;
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
            _visualEffect.Stop();
            _isEnable = false;
        }
    }
}