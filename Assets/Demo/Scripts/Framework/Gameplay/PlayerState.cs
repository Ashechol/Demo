using System.Collections;
using System.Collections.Generic;
using Demo.Framework.FSM;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class PlayerState : State
    {
        protected readonly PlayerController _player;
        
        public PlayerState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine)
        {
            _player = player;
        }
    }
}
