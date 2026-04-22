using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.Jiwon.Code.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/Stat", order = 0)]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float previous);
        public event ValueChangeHandler OnValueChange;

        public string statName;
        [SerializeField] private float baseValue, minValue, maxValue;
        private float _modifiedValue = 0;
        
        private Dictionary<object, float> _modifyDictionary = new Dictionary<object, float>();

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float pervValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangedEvent(Value, pervValue);
            }
        }

        private void TryInvokeValueChangedEvent(float current, float prevValue)
        {
            if (!Mathf.Approximately(current, prevValue))
            {
                OnValueChange?.Invoke(this, current, prevValue);
            }
        }

        public float Value => Mathf.Clamp(baseValue + _modifiedValue, minValue, maxValue);
        public bool IsMin => Mathf.Approximately(Value, minValue);
        public bool IsMax => Mathf.Approximately(Value, maxValue);

        public void AddModifier(object key, float value)
        {
            if (_modifyDictionary.ContainsKey(key)) return;
            float pervValue = Value;
            
            _modifiedValue += value;
            _modifyDictionary.Add(key, value);
            
            TryInvokeValueChangedEvent(Value, pervValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyDictionary.TryGetValue(key, out float value))
            {
                float pervValue = Value;
                _modifiedValue -= value;
                _modifyDictionary.Remove(key);
                
                TryInvokeValueChangedEvent(Value, pervValue);
            }
        }

        public void ClearAllModifier()
        {
            float pervValue = Value;
            _modifiedValue = 0;
            _modifyDictionary.Clear();
            TryInvokeValueChangedEvent(Value, pervValue);
        }


        public object Clone() => Instantiate(this);
    }
}