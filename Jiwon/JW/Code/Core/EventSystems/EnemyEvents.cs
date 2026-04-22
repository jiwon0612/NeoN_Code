using Work.JW.Code.Enemies;

namespace Work.JW.Code.Core.EventSystems
{
    public class EnemyEvents
    {
        public static readonly EnemyDeadEvent EnemyDeadEvent = new EnemyDeadEvent();
    }
    
    public class EnemyDeadEvent : GameEvent
    {
        
    }
}