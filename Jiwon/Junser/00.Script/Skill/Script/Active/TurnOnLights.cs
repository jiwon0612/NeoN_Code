using UnityEngine;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Entities;
public class TurnOnLights : ActiveSkill
{
    [SerializeField] private PoolTypeSO _flash;
    [SerializeField] private PoolManagerSO poolManager;
    [SerializeField] private float duration, disableDuration;
    [SerializeField] private Canvas canvas;

    private SkillCompo _skillCompo;

    public override void Initialize(Entity entity)
    {
        _skillCompo = entity.GetCompo<SkillCompo>();
    }
    [ContextMenu("dfadf")]
    public override void UseSkill()
    {
        FlashCompo flash = poolManager.Pop(_flash) as FlashCompo;
        if(flash.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            rectTransform.SetParent(canvas.transform);
            rectTransform.offsetMin = new Vector2(0,0);
            rectTransform.offsetMax = new Vector2(0,0);
            flash.transform.position = Vector3.zero;
            flash.StartFlash(duration, disableDuration);
        }
    }
}
