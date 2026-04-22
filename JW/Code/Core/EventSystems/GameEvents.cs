using Work.JW.Code.Entities;

namespace Work.JW.Code.Core.EventSystems
{
    public class GameEvents
    {
        public static readonly WaveChangeEvent WaveChangeEvent = new WaveChangeEvent();
        public static readonly GameStopEvent GameStopEvent = new GameStopEvent();
        public static readonly GameOverEvent GameOverEvent = new GameOverEvent();
        public static readonly BossSpawnEvent BossSpawnEvent = new BossSpawnEvent();
    }

    public class WaveChangeEvent : GameEvent
    {
        public int wave;
        
        public WaveChangeEvent Initializer(int num)
        {
            wave = num;
            return this;
        }
    }

    public class GameStopEvent : GameEvent
    {
        public bool isStop;
        
        public GameStopEvent Initializer(bool stop)
        {
            isStop = stop;
            return this;
        }
    }
    
    public class GameOverEvent : GameEvent {}

    public class BossSpawnEvent : GameEvent
    {
        public Entity boss;
        
        public BossSpawnEvent Initializer(Entity owner)
        {
            boss = owner;
            return this;
        }
    }
}