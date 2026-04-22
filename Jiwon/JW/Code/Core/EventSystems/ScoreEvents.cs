using System.Collections.Generic;

namespace Work.JW.Code.Core.EventSystems
{
    public static class ScoreEvents
    {
        public static readonly ChangeHighScoreEvent ChangeHighScoreEvent = new ChangeHighScoreEvent();
        public static readonly RefreshRankingEvent RefreshRankingEvent = new RefreshRankingEvent();
        public static readonly ChangeRankingEvent ChangeRankingEvent = new ChangeRankingEvent();
        public static readonly AddScoreEvent AddScoreEvent = new AddScoreEvent();
        public static readonly LoadUserScoreEvent LoadUserScoreEvent = new LoadUserScoreEvent();
        public static readonly ChangeUserNameEvent ChangeUserNameEvent = new ChangeUserNameEvent();
        public static readonly WarningChangeUserNameEvent WarningChangeUserNameEvent = new WarningChangeUserNameEvent();
        public static readonly SaveUserScoreEvent SaveUserScoreEvent = new SaveUserScoreEvent();
    }

    public class AddScoreEvent : GameEvent
    {
        public int score;
        
        public AddScoreEvent Initializer(int score)
        {
            this.score = score;
            return this;
        }
    }
    
    //아이디랑 스코어를 Handler가 전달받아서 직접 최고 점수인지 확인해줘야함
    public class ChangeHighScoreEvent : GameEvent
    {
        public int score;
        public string userId;

        public ChangeHighScoreEvent Initializer(string id, int score)
        {
            userId = id;
            this.score = score;
            return this;
        }
    }
    
    public class RefreshRankingEvent : GameEvent
    {        
    }

    public class LoadUserScoreEvent : GameEvent
    {
        public int userScore;
        public string userId;
        
        public LoadUserScoreEvent Initializer(string id, int score)
        {
            userId = id;
            userScore = score;
            return this;
        }
    }

    public class ChangeRankingEvent : GameEvent
    {
        public Dictionary<string, int> rankings;
        
        public ChangeRankingEvent Initializer(Dictionary<string, int> data)
        {
            rankings = data;
            return this;
        }
    }

    public class ChangeUserNameEvent : GameEvent
    {
        public string userName;
        public ChangeUserNameEvent Initializer(string name)
        {
            userName = name;
            return this;
        }
    }

    public class WarningChangeUserNameEvent : GameEvent
    {
        public string warningMessage;
        public WarningChangeUserNameEvent Initializer(string message)
        {
            warningMessage = message;
            return this;
        }
    }

    public class SaveUserScoreEvent : GameEvent
    {
        public bool isCompleted;
        
        public SaveUserScoreEvent Initializer(bool completed)
        {
            isCompleted = completed;
            return this;
        }
    }
}