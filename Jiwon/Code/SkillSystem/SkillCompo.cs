using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem
{
    public class SkillCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO skillChannel;
        [field: SerializeField] public PoolManagerSO poolManager;
        [field: SerializeField] public List<SkillDataSO> skillList;
        private Dictionary<string, Skill> _skillDictionary;
        
        [field:SerializeField] public ActiveSkill CurrentActiveSkill { get; private set; }
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            
            _skillDictionary = new Dictionary<string, Skill>();
            GetComponentsInChildren<Skill>(true).ToList().ForEach(
                skill =>
                {
                    skill.Initialize(_entity);
                    SkillDataSO skillData = skillList.Find(b => b.skillName == skill.name);
                    Debug.Assert(skillData != null,$"{skill.name} SkillData is null");
                    skillData.skill = skill;
                    _skillDictionary.Add(skill.name, skill);
                });
        }

        public bool UseActiveSkill()
        {
            if (CurrentActiveSkill == null) return false;
            
            CurrentActiveSkill.UseSkill();
            skillChannel.RaiseEvent(SkillEvents.UseSkillEvent.Initialize(CurrentActiveSkill.SkillData));
            
            CurrentActiveSkill = null;
            return true;
        }

        private bool SetActiveSkill(SkillDataSO key)
        {
            Skill skill = _skillDictionary.GetValueOrDefault(key.skillName);
            if (skill.GetType().IsSubclassOf(typeof(ActiveSkill)))
            {
                CurrentActiveSkill = skill as ActiveSkill;
                return true;
            } 
            return false;
        }

        private bool SetPassiveSkill(SkillDataSO key)
        {
            
            Skill skill = _skillDictionary.GetValueOrDefault(key.skillName);
            if (skill.GetType().IsSubclassOf(typeof(PassiveSkill)))
            {
                PassiveSkill passiveSkill = skill as PassiveSkill;
                passiveSkill.ActiveSkill();
                return true;
            }
            return false;
        }

        public void SetSkill(SkillDataSO key)
        {
            skillChannel.RaiseEvent(SkillEvents.GetSkillEvent.Initialize(key));
            if (SetPassiveSkill(key))
            {
                Debug.Log("passive skill set");
            }

            if (SetActiveSkill(key))
            {
                Debug.Log("active skill set");
            }
        }
    }
}