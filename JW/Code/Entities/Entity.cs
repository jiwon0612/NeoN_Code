using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Work.JW.Code.Enemies;
using Work.JW.Code.Firebase.RankingBoard;

namespace Work.JW.Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent OnDeadEvent;
        public bool IsDead { get; set; }
        
        private Dictionary<Type, IEntityComponent> _components;
        private Dictionary<Type, IAfterInit> _componentAfters;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            _componentAfters = new Dictionary<Type, IAfterInit>();

            AddComponents();
            InitComponents();
            AfterInitComponents();
        }

        public T GetCompo<T>(bool isDreived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
                return (T)component;
            
            if (!isDreived) return default;
            
            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
                return (T)_components[findType];
            
            return default(T);
        }

        protected virtual void AddComponents()
        {
            // GetComponentsInChildren<IEntityComponent>(true).ToList()
            //     .ForEach(compo => _components.Add(compo.GetType(), compo));

            foreach (var compo in GetComponentsInChildren<IEntityComponent>(true))
            {
                _components.Add(compo.GetType(), compo);
            }
            
            GetComponentsInChildren<IAfterInit>(true).ToList()
                .ForEach(after => _componentAfters.Add(after.GetType(), after));
        }

        protected virtual void InitComponents()
        {
            //_components.Values.ToList().ForEach(compo => compo.Initialize(this));
            foreach (var component in _components.Values)
            {
                component.Initialize(this);
            }
        }

        protected virtual void AfterInitComponents()
        {
            _componentAfters.Values.ToList().ForEach(after => after.AfterInitialize());
        }

        public virtual void OnDead()
        {
            IsDead = true;
            OnDeadEvent?.Invoke();
        }
    }
}