using UnityEngine;

namespace Work.JW.Code.Effects
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        [SerializeField] private GameObject effectObject;

        public GameObject GameObject => gameObject;
        
        private IPlayableVFX _playableVFX;
        private Pool _myPool;

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
        }

        public void ResetItem()
        {
            
        }

        private void OnValidate()
        {
            if(effectObject == null) return;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            if (_playableVFX == null)
            {
                Debug.LogError($"The effect object {effectObject.name} does not implement IPlayableVFX.");
                effectObject = null;
            }
        }

        public void PlayVFX(Vector3 hitPoint, Quaternion rotation)
        {
            _playableVFX.PlayVfx(hitPoint, rotation);
        }
    }
}