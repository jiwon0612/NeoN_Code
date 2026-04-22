using Work.JW.Code.Entities;
using Work.JW.Code.FSM;

namespace Work.Jiwon.Code.Player.State
{
    public class PlayerState : EntityState
    {
        protected EntityMover _mover;
        protected Player _player;
        
        public PlayerState(Entity entity, int animHash) : base(entity, animHash)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Update()
        {
            base.Update();
            
            if (_player.IsStop)
                _player.StateChange("STOP");
            
            _mover.WarpTo(_player.PlayerInput.GetWoldPosition());
        }
    }
}