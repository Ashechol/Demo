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
            
            _character.Move(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Locomotion();
            
            _character.anim.UpdateMoveParam(_character.CurSpeed, _character.RotationSpeedRef);
        }

        private void Locomotion()
        {
            if (_input.IsMoveInput)
            {
                var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _player.cameraYaw;
                _character.Turn(targetAngle);

                var targetSpeed = _character.runSpeed;
                if (_input.DashInput) targetSpeed = _character.dashSpeed;
                _character.Move(targetSpeed);
            }
            else
            {
                _stateMachine.ChangeState(_player.idleState);
            }
        }
    }
}
