using System.Collections;
using System.Collections.Generic;
using Demo.Framework.Gameplay;
using DG.Tweening;
using UnityEngine;

namespace Demo
{
    public class PlayerLandingState : PlayerGroundState
    {
        public PlayerLandingState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayLanding();
            _character.anim.UpdateLandingParam(_character.CurSpeed, _character.FallSpeed); // Play 后才会创建 State
        }

        public override void Exit()
        {
            _character.Move(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _player.cameraYaw;
            _character.Turn(targetAngle);
            _character.Move(_character.anim.AnimDesiredSpeed);
        }
    }
}
