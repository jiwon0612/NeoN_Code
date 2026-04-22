using Ami.BroAudio.Data;
using UnityEngine;
using Ami.BroAudio;
using DG.Tweening;

namespace Work.JW.Code.SoundSystem
{
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        private Pool _myPool;
        
        [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
        [SerializeField] private float duration = 2f;
        public GameObject GameObject => gameObject;
        

        public void PlaySound(SoundID clip)
        {
            BroAudio.Play(clip);

            DOVirtual.DelayedCall(duration, () => _myPool.Push(this));
        }

        public void StopSound(SoundID clip)
        {
            BroAudio.Stop(clip);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            
        }
    }
}