using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using Work.JW.Code.SoundSystem;

namespace Work.JW.Code.Feedbacks
{
    public class PlaySoundFeedback : Feedback
    {
        [SerializeField] private SoundID clip;
        [SerializeField] private PoolTypeSO soundPlayerItem;
        [SerializeField] private PoolManagerSO poolM;
        private SoundPlayer _sound;
        
        public override void CreateFeedback()
        {
            _sound = poolM.Pop(soundPlayerItem) as SoundPlayer;
            
            _sound.PlaySound(clip);
        }

        public override void StopFeedback()
        {
            
        }
    }
}