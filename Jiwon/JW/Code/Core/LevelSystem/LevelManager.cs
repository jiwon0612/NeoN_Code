using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.Core.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        public UnityEvent OnLevelUpEvent;
        
        [SerializeField] private GameEventChannelSO levelChannel;
        [SerializeField] private GameEventChannelSO gameChannel;

        private int _level;
        
        [SerializeField] private float wantExp;
        public float WantExp => wantExp;
        [SerializeField] private float nextWantExpPercent;

        private float _exp;
        public float Exp
        {
            get => _exp;
            set
            {
                _exp = value;

                while (_exp >= wantExp)
                {
                    _exp -= wantExp;
                    _level++;
                    
                    var levelUpEvt = LevelEvents.LevelUpEvent.Initializer(_level);
                    levelChannel.RaiseEvent(levelUpEvt);
                    
                    OnLevelUpEvent?.Invoke();
                    
                    var gameStopEvt = GameEvents.GameStopEvent.Initializer(true);
                    gameChannel.RaiseEvent(gameStopEvt);

                    wantExp *= nextWantExpPercent;
                }
                
                var changeEvt = LevelEvents.EXPChangeEvent.Initializer(_exp, wantExp);
                levelChannel.RaiseEvent(changeEvt);
            }
        }

        private void Awake()
        {
            levelChannel.AddListener<AddEXPEvent>(HandleAddExp);
        }

        private void HandleAddExp(AddEXPEvent evt)
        {
            Exp += evt.exp;
        }

        private void OnDestroy()
        {
            levelChannel.RemoveListener<AddEXPEvent>(HandleAddExp);
        }
        
        [ContextMenu("TestAddExp")]
        private void TestAddExp()
        {
            var evt = LevelEvents.AddEXPEvent.Initializer(2);
            levelChannel.RaiseEvent(evt);
        }
    }
}