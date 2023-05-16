using System.Collections;
using System.Collections.Generic;
using Demo.Framework.FSM;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.character.anim.PlayIdle();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_player.input.IsMoveInput)
                _stateMachine.ChangeState(_player.moveState);
        }
    }
}
