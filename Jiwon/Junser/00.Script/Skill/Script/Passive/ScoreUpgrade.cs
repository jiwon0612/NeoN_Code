using UnityEngine;
using Work.Jiwon.Code.SkillSystem;
public class ScoreUpgrade:PassiveSkill
{
    public override void ActiveSkill()
    {
        base.ActiveSkill();
        ScoreManager.Instance.OnScoreDoubleSkill(2f);
    }
    public override void DisableSkill()
    {
        ScoreManager.Instance.OnScoreDoubleSkill(0.5f);
        base.DisableSkill();
    }
}
