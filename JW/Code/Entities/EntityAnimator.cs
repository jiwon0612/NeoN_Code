using UnityEngine;

namespace Work.JW.Code.Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private Animator Animator;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;

            Animator = entity.GetComponentInChildren<Animator>();
        }

        #region Param section

        public void SetParam(int param, bool value) => Animator.SetBool(param, value);
        public void SetParam(int param, int value) => Animator.SetInteger(param, value);
        public void SetParam(int param, float value) => Animator.SetFloat(param, value);
        public void SetParam(int param) => Animator.SetTrigger(param);

        #endregion
    }
}