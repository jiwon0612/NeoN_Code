using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.JW.Code.FSM
{
    public abstract class EntityState
    { 
        protected Entity _entity;
        protected EntityAnimator _entityAnim;
        protected EntityAnimatorTrigger _entityAnimTrigger;
        
        protected int _animHash;
        protected bool _isTriggerCall;
        
        public EntityState(Entity entity, int animHash)
        {
            _entity = entity;
            _animHash = animHash;

            _entityAnim = entity.GetCompo<EntityAnimator>();
            _entityAnimTrigger = entity.GetCompo<EntityAnimatorTrigger>(true);
        }
        
        public virtual void Enter()
        {
            _isTriggerCall = false;
            _entityAnim?.SetParam(_animHash, true);
            _entityAnimTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }
        
        public virtual void Update()
        {
            
        }
        
        public virtual void Exit()
        {
            _entityAnim?.SetParam(_animHash, false);
            _entityAnimTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
        }

        protected void AnimationEndTrigger() => _isTriggerCall = true;
    }
}