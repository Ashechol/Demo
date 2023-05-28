using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerJumpState : PlayerState
    {
        private bool _isSecondJump;
        
        public PlayerJumpState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            if (_character.IsGrounded)
                _character.anim.PlayJump();
            else
            {
                _character.anim.PlaySecondJump();
                _isSecondJump = true;
            }    
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // _character.Jump(_);
            
            if (!_character.IsGrounded && !_isSecondJump)
                _stateMachine.ChangeState(_player.airBorneState);
            else if (_character.anim.IsAnimExiting(0.5f))
            {
                _stateMachine.ChangeState(_player.airBorneState);
                _isSecondJump = false;
            }
        }
    }
}
