using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.Core.EventSystems
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "SO/Events/Channel", order = 0)]
    public class GameEventChannelSO : ScriptableObject
    {
        private Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();
        private Dictionary<Delegate, Action<GameEvent>> _lookupTable = new Dictionary<Delegate, Action<GameEvent>>();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookupTable.ContainsKey(handler))
            {
                Debug.LogWarning("Handler already added");
                return;
            }
            
            Action<GameEvent> castHandler = evt => handler.Invoke(evt as T);
            _lookupTable[handler] = castHandler;
            
            Type evtType = typeof(T);
            if (_events.ContainsKey(evtType))
                _events[evtType] += castHandler;
            else
                _events[evtType] = castHandler;
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lookupTable.TryGetValue(handler, out Action<GameEvent> castHandler))
            {
                if (_events.TryGetValue(evtType, out Action<GameEvent> evtHandler))
                {
                    evtHandler -= castHandler;
                    if (evtHandler == null)
                        _events.Remove(evtType);
                    else 
                        _events[evtType] = evtHandler;
                }
                _lookupTable.Remove(handler);
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out Action<GameEvent> castHandler))
                castHandler?.Invoke(evt);
        }

        public void Clear()
        {
            _events.Clear();
            _lookupTable.Clear();
        }
    }

    public abstract class GameEvent
    {
    }
}