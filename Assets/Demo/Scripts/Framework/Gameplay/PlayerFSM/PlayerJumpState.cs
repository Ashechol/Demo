using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayJump();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _character.Jump();
            
            if (!_character.IsGrounded)
                _stateMachine.ChangeState(_player.airBorneState);
        }
    }
}
