namespace Work.JW.Code.Core.EventSystems
{
    public static class LevelEvents
    {
        public static readonly AddEXPEvent AddEXPEvent = new AddEXPEvent();
        public static readonly LevelUpEvent LevelUpEvent = new LevelUpEvent();
        public static readonly EXPChangeEvent EXPChangeEvent = new EXPChangeEvent();
    }

    public class AddEXPEvent : GameEvent
    {
        public float exp;

        public AddEXPEvent Initializer(float exp)
        {
            this.exp = exp;
            return this;
        }
    }

    public class EXPChangeEvent : GameEvent
    {
        public float exp;
        public float wantExp;

        public EXPChangeEvent Initializer(float exp, float wantExp)
        {
            this.exp = exp;
            this.wantExp = wantExp;
            return this;
        }
    }
    
    public class LevelUpEvent : GameEvent
    {
        public int level;
        
        public LevelUpEvent Initializer(int level)
        {
            this.level = level;
            return this;
        }
    }
}