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
            
            if (!_player.combat.IsWeaponDrawn)
                _character.anim.PlayIdle();
            else
                _character.anim.PlayIdleWeapon();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_player.input.IsMoveInput)
                _stateMachine.ChangeState(_player.moveState);
            
            if (_player.input.DrawSheathInput)
                _stateMachine.ChangeState(_player.transitionState.SetType(PlayerTransitionType.DrawSheathStand));
        }
    }
}
