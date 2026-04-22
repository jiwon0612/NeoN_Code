using DG.Tweening;
using UnityEngine;

namespace Work.Jiwon.Code.Combts
{
    public class ParticlePlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private float duration;
        [field:SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        
        private ParticleSystem _particleSystem;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void PlayParticle(Vector3 hitPoint, Quaternion rotation)
        {
            transform.position = hitPoint;
            transform.rotation = rotation;
            _particleSystem.Play();
            DOVirtual.DelayedCall(duration, () => _myPool.Push(this));
        }

        public void ResetItem()
        {
            _particleSystem.Stop();
        }
    }
}