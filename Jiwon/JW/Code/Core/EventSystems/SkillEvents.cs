using Work.Jiwon.Code.SkillSystem;

namespace Work.JW.Code.Core.EventSystems
{
    public static class SkillEvents
    {
        public static readonly BulletBlockModeEvent BulletBlockModeEvent = new BulletBlockModeEvent();
        public static readonly GetSkillEvent GetSkillEvent = new GetSkillEvent();
        public static readonly UseSkillEvent UseSkillEvent = new UseSkillEvent();

    }

    public class BulletBlockModeEvent : GameEvent
    {
        public bool isModeActive;
        public string skillName;
        
        public BulletBlockModeEvent Initializer(bool isActive, string name)
        {
            isModeActive = isActive;
            skillName = name;
            return this;
        }
        
    }
    
    public class GetSkillEvent : GameEvent
    {
        public SkillDataSO skill;
        
        public GetSkillEvent Initialize(SkillDataSO skill)
        {
            this.skill = skill;
            return this;
        }
    }

    public class UseSkillEvent : GameEvent
    {
        public SkillDataSO skill;

        public UseSkillEvent Initialize(SkillDataSO skill)
        {
            this.skill = skill;
            return this;
        }
    }
}