using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Base.PlayerController
{
    public class PlayerGroundState : PlayerState
    {
        protected PlayerGroundState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!_character.IsGrounded)
                _stateMachine.ChangeState(_player.airBorneState);
        }
    }
}
