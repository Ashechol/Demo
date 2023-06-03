using System.Collections;
using System.Collections.Generic;
using Demo.Framework.FSM;
using UnityEngine;

namespace Demo.Base.PlayerController
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _character.anim.PlayIdle(0, _combat.IsWeaponDraw);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_player.input.IsMoveInput)
                _stateMachine.ChangeState(_player.moveState);
        }
    }
}
