using UnityEngine;

namespace Work.JW.Code.Enemies.EnemyWeapons.Bullets.BulletModes
{
    public class FollowBulletMode : BulletMode
    {
        private Transform _targetTrm;

        public override void Enter(Bullet owner)
        {
            base.Enter(owner);
            _targetTrm = Owner.GetShootOwner().transform;
        }

        public override void Update()
        {
            if (!_targetTrm) return;
            
            Vector3 dir = _targetTrm.position - Owner.transform.position;
            Owner.SetDirection(dir.normalized);
        }

        public override void Exit()
        {
            _targetTrm = null;
            base.Exit();
        }
    }
}