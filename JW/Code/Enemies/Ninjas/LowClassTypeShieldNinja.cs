using Work.JW.Code.Enemies.EnemyWeapons.Shields;

namespace Work.JW.Code.Enemies.Ninjas
{
    public class LowClassTypeShieldNinja : LowClassNinja
    {
        private ShieldManager _shieldM;

        protected override void Awake()
        {
            base.Awake();
            _shieldM = GetCompo<ShieldManager>();
        }

        public override void ResetItem()
        {
            base.ResetItem();
            _shieldM.SpawnShield();
            _shieldM.InitShield(transform);
        }

        public override void OnDead()
        {
            _shieldM.OnDead();
            
            base.OnDead();
        }
    }
}