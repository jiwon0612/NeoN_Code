using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.Jiwon.Code.Player.State
{
    public class PlayerStopState : PlayerState
    {
        public PlayerStopState(Entity entity, int animHash) : base(entity, animHash)
        {
        }

        public override void Update()
        {
            if (!_player.IsStop)
                _player.StateChange("IDLE");
        }
    }
}