using System;
using UnityEngine;
using Work.JW.Code.Core.EventSystems;

namespace Work.JW.Code.Enemies.EnemyWeapons.Bullets
{
    public abstract class BulletMode : MonoBehaviour, IBulletMode
    {
        public Bullet Owner { get; protected set; }
        [field: SerializeField] public string ModeName { get; private set; }

        public virtual void Enter(Bullet owner)
        {
            Owner = owner;
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            Owner = null;
        }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(ModeName))
                gameObject.name = ModeName;
        }
    }
}