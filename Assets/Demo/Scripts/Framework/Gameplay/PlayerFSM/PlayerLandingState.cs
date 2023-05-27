using System.Collections;
using System.Collections.Generic;
using Demo.Framework.Gameplay;
using DG.Tweening;
using UnityEngine;

namespace Demo
{
    public class PlayerLandingState : PlayerGroundState
    {
        public PlayerLandingState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player) { }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayLanding(); // Play 后才会创建 State
            
            _character.anim.UpdateLandingParam(_character.FallSpeed);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            var exitTime = _input.IsMoveInput ? 0.15f : 0.45f;
            if (_character.anim.IsAnimExiting(exitTime))
                _stateMachine.ChangeState(_player.moveState);
        }
    }
}
