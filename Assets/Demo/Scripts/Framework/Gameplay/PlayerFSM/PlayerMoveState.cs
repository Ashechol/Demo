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
                if (_character.CurSpeed > 7)
                    _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.DashToStand));
                else if (_character.CurSpeed > 4)
                {
                    _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.RunToStand));
                    _character.SetTargetSpeed(0);
                }
                else
                {
                    _stateMachine.ChangeState(_player.idleState);
                    _character.SetTargetSpeed(0);
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

                if (_input.DashInput)
                    _character.SetTargetSpeed(_character.dashSpeed, _character.dashAcceleration);
                else
                {
                    var acc = _character.CurSpeed > _character.runSpeed ? _character.dashAcceleration : _character.acceleration;
                    _character.SetTargetSpeed(_character.runSpeed, acc);
                }
            }
        }
    }
}
