using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Entities.Attacks
{
    public abstract class EntityAttackCompo : MonoBehaviour, IEntityComponent
    {
        public UnityEvent OnAttackHit;
        protected Entity _entity;
        [SerializeField] protected LayerMask targetLayer;
        
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public abstract void Attack();
        public abstract void EndAttack();
    }
}