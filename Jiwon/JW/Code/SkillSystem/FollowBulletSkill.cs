using System.Collections;
using UnityEngine;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Core.EventSystems;
using Work.JW.Code.Entities;

namespace Work.JW.Code.SkillSystem
{
    public class FollowBulletSkill : ActiveSkill
    {
        [SerializeField] private GameEventChannelSO skillChannel;

        [Header("Setting")] [SerializeField] private string modeName;
        [SerializeField] private float duration;

        private bool _isModeActive = false;
        private Coroutine _useSkillTimeCoroutine;
        
        private ParticleSystem _particleSystem;

        public override void Initialize(Entity entity)
        {
            StopUseSkillCoroutine();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public override void UseSkill()
        {
            _isModeActive = true;
            var evt = SkillEvents.BulletBlockModeEvent.Initializer(_isModeActive, modeName);
            skillChannel.RaiseEvent(evt);
            
            StopUseSkillCoroutine();
            _particleSystem.Play();
            _useSkillTimeCoroutine = StartCoroutine(UseSkillTime());
        }

        private IEnumerator UseSkillTime()
        {
            yield return new WaitForSeconds(duration);

            _isModeActive = false;
            _particleSystem.Stop();
            var evt = SkillEvents.BulletBlockModeEvent.Initializer(_isModeActive, modeName);
            skillChannel.RaiseEvent(evt);

            StopUseSkillCoroutine();
        }
        
        private void StopUseSkillCoroutine()
        {
            if(_useSkillTimeCoroutine != null)
                StopCoroutine(_useSkillTimeCoroutine);
        }
    }
}