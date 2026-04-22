using Work.JW.Code.Core.EventSystems;

namespace Work.Jiwon.Code.EventSystem
{
    public static class PlayerEvents
    {
        public static PlayerDeadEvent PlayerDeadEvent = new PlayerDeadEvent();
        public static PlayerHitEvent PlayerHitEvent = new PlayerHitEvent();
    }

    public class PlayerDeadEvent : GameEvent { }
    
    public class PlayerHitEvent : GameEvent
    {
        public float heath;
        public float maxHeath;
        
        public PlayerHitEvent Initialize(float heath, float maxHeath)
        {
            this.heath = heath;
            this.maxHeath = maxHeath;
            return this;
        }
    }
}