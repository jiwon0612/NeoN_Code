using UnityEngine;
using Work.Jiwon.Code.Entities;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.SkillSystem
{
    public abstract class Skill : MonoBehaviour
    {
        public SkillDataSO SkillData { get; private set; }

        protected Entity _entity;
        
        public abstract void Initialize(Entity entity);
    }
}