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

            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (_stateMachine.CurrentState != this)
                    return;
                _stateMachine.ChangeState(_player.idleState);
            });
        }
    }
}
