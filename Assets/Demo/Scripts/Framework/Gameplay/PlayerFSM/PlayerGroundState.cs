using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerGroundState : PlayerState
    {
        protected PlayerGroundState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_input.JumpInput && _character.TryJump())
                _stateMachine.ChangeState(_player.jumpState);
        }
    }
}
