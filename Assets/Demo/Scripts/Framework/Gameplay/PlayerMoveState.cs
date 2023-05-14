using System.Collections;
using System.Collections.Generic;
using Demo.Framework.FSM;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            // _player.character.anim.PlayMove();
        }
    }
}
