using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem.Skills.ShieldSkills
{
    public class ShieldSkill : ActiveSkill
    {
        public UnityEvent OnUseShieldSEvent;
        
        [SerializeField] private float duration; 
        [SerializeField] private PoolTypeSO shield;
        [SerializeField] private PoolManagerSO poolManager;
        
        private SkillCompo _skillCompo;
        
        public override void Initialize(Entity entity)
        {
            _entity = entity;
            _skillCompo = entity.GetCompo<SkillCompo>();
        }

        public override void UseSkill()
        {
            OnUseShieldSEvent?.Invoke();
            PlayerShield shieldObj =  poolManager.Pop(shield) as PlayerShield;
            //shieldObj.transform.SetParent(transform);
            shieldObj.transform.position = _entity.transform.position;
            shieldObj.transform.localPosition = Vector3.up * 1.5f;
            shieldObj.StartShield(duration, _entity);
        }
    }
}