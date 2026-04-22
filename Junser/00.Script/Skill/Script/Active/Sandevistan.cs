using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Entities;
using Work.JW.Code.Enemies.EnemyWeapons.Bullets;

public class Sandevistan : ActiveSkill
{
    public UnityEvent<bool> OnUseSkillEvent; 
    [SerializeField] private float duration;
    private float _defaltTimeScale;
    [SerializeField] SandevistanEffect sandevistan;
    
    
    public override void Initialize(Entity entity)
    {
        _defaltTimeScale = Bullet.speedMultiplier;
    }

    public override void UseSkill()
    {
        Bullet.speedMultiplier = _defaltTimeScale/2;
        OnUseSkillEvent?.Invoke(true);
        sandevistan.Active();
        StartCoroutine(ResetTimeSpeed());
    }

    private IEnumerator ResetTimeSpeed()
    {
        yield return new WaitForSecondsRealtime(duration);
        OnUseSkillEvent?.Invoke(false);
        Bullet.speedMultiplier = _defaltTimeScale;
        sandevistan.Disactive();
    }
}
