using System.Collections;
using System.Collections.Generic;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo
{
    public class PlayerAirBorneState : PlayerState
    {
        private Vector3 _jumpForward;
        private float _targetSpeed;

        public PlayerAirBorneState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayAirBorne();

            _jumpForward = _character.transform.forward;
        }

        public override void Exit()
        {
            base.Exit();
            
            _character.SetTargetSpeed(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            AirBorneLocomotion();
                
            if (_character.IsGrounded)
                _stateMachine.ChangeState(_player.landingState);
            else
                _character.anim.UpdateAirBorneParam(_character.Velocity.y);
        }

        private void AirBorneLocomotion()
        {
            _targetSpeed = 0;
            
            if (_input.IsMoveInput)
            {
                var curForward = _character.transform.forward;
                var angle = Vector3.Angle(_jumpForward, curForward);

                _targetSpeed = angle > 20f ? _character.airSpeed : _character.CurSpeed;

                var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _player.cameraYaw;
                _character.Turn(targetAngle);
            }

            _character.SetTargetSpeed(_targetSpeed);
        }
    }
}
