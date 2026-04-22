using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.JW.Code.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseFeedback : Feedback
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        public override void CreateFeedback()
        {
            impulseSource.GenerateImpulse();

            float duration = impulseSource.ImpulseDefinition.ImpulseDuration;
            DOVirtual.DelayedCall(duration, StopFeedback);
        }

        public override void StopFeedback()
        {
            
        }
    }
}