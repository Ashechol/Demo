using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerMoveState : PlayerGroundState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player) { }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayMove();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Locomotion();

            if (!_input.IsMoveInput)
            {
                switch (_character.CurSpeed)
                {
                    case <= 4.5f:
                        _character.SetTargetSpeed(0);
                        _stateMachine.ChangeState(_player.idleState);
                        break;
                    case <= 10:
                        _character.SetTargetSpeed(0);
                        _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.RunToStand));
                        break;
                    case <= 12:
                        _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.DashToStand));
                        break;
                    default:
                        _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.DashToStand));
                        break;
                }
            }

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
                _character.SetTargetSpeed(targetSpeed);
            }
        }
    }
}
