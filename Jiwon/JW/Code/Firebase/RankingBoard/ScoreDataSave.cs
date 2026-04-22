using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.Firebase.RankingBoard
{
    public class ScoreDataSave : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO scoreChannel;
        [SerializeField] private int topCount;
        private FirebaseDatabase _database;

        private string curUserId;

        private const string USERID_FIELD = "userid";
        private const string SCORE_FIELD = "score";
        private const string LEADERBOARD_FIELD = "leaderboard";

        private async void Awake()
        {
            curUserId = Environment.MachineName;

            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            
            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                var dbUrl = new Uri("https://ninja-5a007-default-rtdb.firebaseio.com/");
                _database = FirebaseDatabase.GetInstance(app, dbUrl.ToString());
                
                _database.SetPersistenceEnabled(false);
                Debug.Log("Firebase Initialize Complete");
            }
            else
            {
                Debug.LogError($"Firebase Initialize fail: {dependencyStatus}");
            }

            scoreChannel.AddListener<ChangeHighScoreEvent>(HandleChangeHighScore);
            scoreChannel.AddListener<ChangeUserNameEvent>(HandleChangeUserNameEvent);
        }

        private void OnDestroy()
        {
            scoreChannel.RemoveListener<ChangeHighScoreEvent>(HandleChangeHighScore);
            scoreChannel.RemoveListener<ChangeUserNameEvent>(HandleChangeUserNameEvent);
        }

        public async Task SaveScore(int score)
        {
            if (_database == null) return;

            var evt = ScoreEvents.SaveUserScoreEvent.Initializer(false);
            scoreChannel.RaiseEvent(evt);

            try
            {
                DatabaseReference reference = _database.RootReference;
                await reference.Child("leaderboard").Child(curUserId).Child(SCORE_FIELD).SetValueAsync(score);

                Debug.Log("Score successfully saved!");
                evt = ScoreEvents.SaveUserScoreEvent.Initializer(true);
                scoreChannel.RaiseEvent(evt);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save score: " + e.Message);
            }
        }

        private async void HandleChangeUserNameEvent(ChangeUserNameEvent evt)
        {
            await ChangeUserName(evt.userName);
        }

        private async Task ChangeUserName(string changeUserName)
        {
            if (_database == null) return;

            var SaveNameEvt = ScoreEvents.SaveUserScoreEvent.Initializer(false);
            scoreChannel.RaiseEvent(SaveNameEvt);

            if (string.IsNullOrEmpty(changeUserName))
            {
                var warningEvt = ScoreEvents.WarningChangeUserNameEvent.Initializer("이름을 입력해주세요.");
                scoreChannel.RaiseEvent(warningEvt);
                return;
            }

            DatabaseReference reference = _database.RootReference;

            //이름 중복 체크
            DataSnapshot snapshot = await reference.Child(LEADERBOARD_FIELD).GetValueAsync();
            
            foreach (var snap in snapshot.Children)
            {
                if (changeUserName.Equals((string)snap.Child(USERID_FIELD).Value))
                {
                    var evt = ScoreEvents.WarningChangeUserNameEvent.Initializer("이미 존재하는 이름입니다.");
                    scoreChannel.RaiseEvent(evt);
                    return;
                }
            }

            try
            {
                await reference.Child(LEADERBOARD_FIELD).Child(curUserId).Child(USERID_FIELD).SetValueAsync(changeUserName);
                
                SaveNameEvt = ScoreEvents.SaveUserScoreEvent.Initializer(true);
                scoreChannel.RaiseEvent(SaveNameEvt);
                Debug.Log("User name successfully changed!");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save user name: " + e);
                var evt = ScoreEvents.WarningChangeUserNameEvent.Initializer("이름 변경 실패");
                scoreChannel.RaiseEvent(evt);
            }
        }

        public async void HandleLoadTopScore()
        {
            try
            {
                await LoadTopScores();
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load top scores");
            }
        }

        private async Task LoadTopScores()
        {
            if (_database == null) return;
            DatabaseReference reference = _database.GetReference(LEADERBOARD_FIELD);
            Dictionary<string, int> results = new Dictionary<string, int>();

            DataSnapshot dataSnapshot =
                await reference.OrderByChild(SCORE_FIELD).LimitToLast(topCount).GetValueAsync();

            foreach (var child in dataSnapshot.Children)
            {
                Debug.Log("In");
                if (!child.HasChild(SCORE_FIELD) || child.Child(SCORE_FIELD).Value == null)
                    continue;
                try
                {
                    Debug.Log(child.Key);

                    string userId = child.Child(USERID_FIELD).Value?.ToString() ?? child.Key;
                    int score = int.Parse(child.Child(SCORE_FIELD).Value.ToString());

                    results.Add(userId, score);
                    Debug.Log($"{userId}: {score}");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error parsing score for user {child.Key}: {e.Message}");
                }
            }

            var sortedResults = results
                .OrderByDescending(kvp => kvp.Value)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var evt = ScoreEvents.ChangeRankingEvent.Initializer(sortedResults);
            scoreChannel.RaiseEvent(evt);
        }

        public async void HandleLoadUserScore()
        {
            var evt = ScoreEvents.LoadUserScoreEvent.Initializer("Loading..", 0);
            scoreChannel.RaiseEvent(evt);

            try
            {
                int score = await LoadUserScore();
            }
            catch (Exception e)
            {
                Debug.Log("No user score found or failed to load.");
            }
        }

        public async Task<int> LoadUserScore()
        {
            if (_database == null) return 0;

            DatabaseReference reference = _database.GetReference(LEADERBOARD_FIELD).Child(curUserId);

            DataSnapshot dataSnap = await reference.GetValueAsync();

            if (dataSnap.Exists)
            {
                string userId = dataSnap.Child(USERID_FIELD).Value?.ToString() ?? curUserId;

                if (!dataSnap.HasChild(SCORE_FIELD) || dataSnap.Child(SCORE_FIELD).Value == null)
                {
                    Debug.LogWarning("Score field missing or null for user: " + curUserId);

                    var loadUserEvt = ScoreEvents.LoadUserScoreEvent.Initializer(userId, 0);
                    scoreChannel.RaiseEvent(loadUserEvt);

                    return 0;
                }

                int curUserScore = int.Parse(dataSnap.Child(SCORE_FIELD).Value.ToString());

                var evt = ScoreEvents.LoadUserScoreEvent.Initializer(userId, curUserScore);
                scoreChannel.RaiseEvent(evt);

                return curUserScore;
            }

            return 0;
        }

        private async void HandleChangeHighScore(ChangeHighScoreEvent evt)
        {
            await ChangeHighScore(evt.score);
        }

        private async Task ChangeHighScore(int score)
        {
            int currentHighScore = await LoadUserScore();

            if (currentHighScore < score)
                await SaveScore(score);
        }

        [ContextMenu("Save current User Score")]
        private void TestSaveCurrentUserScore()
        {
            int score = UnityEngine.Random.Range(0, 1000);
            SaveScore(score);
        }
    }

    public struct TopUserData
    {
        public string userId;
        public int score;

        public TopUserData(string id, int score)
        {
            userId = id;
            this.score = score;
        }
    }
}