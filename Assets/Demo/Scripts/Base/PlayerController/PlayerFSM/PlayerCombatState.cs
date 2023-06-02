using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Base.PlayerController
{
    public class PlayerCombatState : PlayerState
    {
        protected PlayerCombatState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }
    }
}
