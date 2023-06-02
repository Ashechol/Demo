using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Base.PlayerController
{
    public class PlayerJumpState : PlayerState
    {
        private bool _isSecondJump;
        private float _speed;

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
                _speed = _character.CurSpeed + 3.5f;
                _isSecondJump = true;
            }    
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!_isSecondJump)
                _stateMachine.ChangeState(_player.airBorneState);
            else if (_character.anim.IsAnimExiting())
            {
                _stateMachine.ChangeState(_player.airBorneState);
                _isSecondJump = false;
            }
            else if (_isSecondJump)
            {
                if (_input.IsMoveInput)
                {
                    var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _player.cameraYaw;
                    _character.Turn(targetAngle);
                }
                
                _character.SetTargetSpeed(_input.IsMoveInput ? _speed : 0);
            }
        }
    }
}
