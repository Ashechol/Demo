using System.Collections;
using System.Collections.Generic;
using Demo.Framework.FSM;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerMoveState : PlayerGroundState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayMove();
        }

        public override void Exit()
        {
            base.Exit();
            
            _character.SetTargetSpeed(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Locomotion();
            
            // if (!_input.IsMoveInput)
            //     _stateMachine.ChangeState(_player.idleState);

            if (!_input.IsMoveInput)
            {
                var type = _character.CurSpeed is > 2 and < 5.01f
                    ? PlayerTransitionType.RunToStand : PlayerTransitionType.DashToStand;
                
                _stateMachine.ChangeState(_player.transitionState.SetType(type));
            }

            _character.anim.UpdateMoveParam(_character.CurSpeed, _character.RotationSpeedRef);
        }

        internal void Locomotion()
        {
            if (_input.IsMoveInput)
            {
                var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _player.cameraYaw;
                _character.Turn(targetAngle);

                var targetSpeed = _character.runSpeed;
                if (_input.DashInput) targetSpeed = _character.dashSpeed;
                _character.SetTargetSpeed(targetSpeed);
            }
        }
    }
}
