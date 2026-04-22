using System;
using DG.Tweening;
using UnityEngine;

namespace Work.JW.Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        
        public event Action OnAnimationEndTrigger;

        private void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void OnEnable()
        {
            transform.DOShakePosition(0.1f, 1);
        }
    }
}