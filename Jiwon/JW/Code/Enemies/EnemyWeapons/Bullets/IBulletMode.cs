namespace Work.JW.Code.Enemies.EnemyWeapons.Bullets
{
    public interface IBulletMode
    {
        public Bullet Owner { get; }
        public string ModeName { get; }
        public void Enter(Bullet owner);
        public void Update();
        public void Exit();
    }
}