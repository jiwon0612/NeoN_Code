using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

namespace Work.Jiwon.Code.Combts
{
    public class VFXPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private float duration;
        [field:SerializeField] public PoolTypeSO PoolType { get; private set; }
        public GameObject GameObject => gameObject;
        
        private Pool _myPool;
        private VisualEffect _visualEffect;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _visualEffect = GetComponent<VisualEffect>();
        }

        public void PlayVFX(Vector3 hitPoint, Quaternion rotation)
        {
            transform.position = hitPoint;
            transform.rotation = rotation;
            _visualEffect.Play();
            DOVirtual.DelayedCall(duration, () => _myPool.Push(this));
        }

        public void ResetItem()
        {
            _visualEffect.Stop();
        }
    }
}