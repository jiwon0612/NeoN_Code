using UnityEngine;

namespace Work.Jiwon.Code.SkillSystem
{
    [CreateAssetMenu(fileName = "SkillDataSO", menuName = "SO/Skill/SkillData", order = 0)]
    public class SkillDataSO : ScriptableObject
    {
        [Header("SkillInfo")]
        public string skillName;
        [TextArea]
        public string description;
        public Sprite icon;
        public bool isActive;
        
        [HideInInspector] public Skill skill;
    }
}