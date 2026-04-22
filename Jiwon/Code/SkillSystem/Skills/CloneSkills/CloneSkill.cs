using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.CloneSkills
{
    public class CloneSkill : ActiveSkill
    {
        public UnityEvent OnUseCloneEvent;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO cloneItem;
        [SerializeField] private int cloneCount;
        [SerializeField] private float cloneLength;
        private Clone[] _clones;
        
        public override void Initialize(Entity entity)
        {
            _entity = entity;
            _clones = new Clone[cloneCount];
        }

        public override void UseSkill()
        {
            OnUseCloneEvent?.Invoke();
            
            for (int i = 0; i < cloneCount; i++)
            {
                if (_clones[i] == null) continue;
                
                _clones[i].OnDead();
                _clones[i] = null;
            }
            
            float angle = (180 / (float)(cloneCount + 1)) * Mathf.Deg2Rad;
            for (int i = 0; i < cloneCount; i++)
            {
                Clone clone = poolManager.Pop(cloneItem) as Clone;
                Vector3 offset = new Vector3(Mathf.Cos(angle * (i + 1)), 0, Mathf.Sin(angle * (i + 1)));
                clone.InitClone(offset * cloneLength);
                _clones[i] = clone;
            }
        }
    }
}