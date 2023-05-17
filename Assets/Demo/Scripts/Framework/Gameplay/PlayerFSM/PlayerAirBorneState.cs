using System.Collections;
using System.Collections.Generic;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo
{
    public class PlayerAirBorneState : PlayerState
    {
        public PlayerAirBorneState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayAirBorne();
        }

        public override void Exit()
        {
            base.Exit();
            
            _character.Move(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _character.anim.UpdateAirBorneParam(_character.Velocity.y);
            
            if (_character.IsGrounded)
                _stateMachine.ChangeState(_player.landingState);
        }
    }
}
