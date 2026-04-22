using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

namespace Work.Jiwon.Code.SkillSystem.Skills.ShieldSkills
{
    public class PlayerShieldEffect : MonoBehaviour, IPoolable
    {
        [SerializeField] private float duration;
        [field:SerializeField] public PoolTypeSO PoolType { get; private set; }

        private readonly int _shperCenter = Shader.PropertyToID("SphereCenter");
        
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        private VisualEffect _visualEffect;
            
        private void Awake() =>  _visualEffect = GetComponent<VisualEffect>();

        public void StartEffect(Vector3 point)
        {
            _visualEffect.Play();  
            _visualEffect.SetVector3(_shperCenter, point);

            DOVirtual.DelayedCall(duration, () => _myPool.Push(this));
        } 
        
        public void SetUpPool(Pool pool) => _myPool = pool;
        
        public void ResetItem() => _visualEffect.Stop();
    }
}