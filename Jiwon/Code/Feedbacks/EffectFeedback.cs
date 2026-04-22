using UnityEngine;
using Work.JW.Code.Feedbacks;

namespace Work.Jiwon.Code.Feedbacks
{
    public class EffectFeedback : Feedback
    {
        [SerializeField] private PoolTypeSO effectPoolType;
        [SerializeField] private float duration;
        
        public override void CreateFeedback()
        {
            
        }

        public override void StopFeedback()
        {
            
        }
    }
}