using UnityEngine;

using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Entities;
public class SmallPlayer:PassiveSkill
{
    Vector3 originSize;
    [SerializeField]
    private float scale = 0.5f;
    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        originSize = entity.transform.localScale;
    }
    public override void ActiveSkill()
    {
        base.ActiveSkill();
        _entity.transform.localScale = originSize * scale;
    }
    public override void DisableSkill()
    {
        _entity.transform.localScale = originSize;
        base.DisableSkill();
    }
}
