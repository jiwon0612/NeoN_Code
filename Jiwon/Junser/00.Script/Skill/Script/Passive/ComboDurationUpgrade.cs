using UnityEngine;

namespace Work.Jiwon.Code.SkillSystem
{
    public class ComboDurationUpgrade : PassiveSkill
    {
        public override void ActiveSkill()
        {
            base.ActiveSkill();
            ScoreManager.Instance.OnComboDurationUpgrade(2);
        }

        public override void DisableSkill()
        {
            ScoreManager.Instance.OnComboDurationUpgrade(0.5f);
            base.DisableSkill();
        }
    }
}