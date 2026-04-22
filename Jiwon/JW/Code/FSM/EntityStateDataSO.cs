using System;
using UnityEngine;

namespace Work.JW.Code.FSM
{
    [CreateAssetMenu(fileName = "EntityStateSO", menuName = "SO/FSM/Data", order = 0)]
    public class EntityStateDataSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        
        public int animHash;

        private void OnValidate()
        {
            animHash = Animator.StringToHash(animParamName);
        }
    }
}