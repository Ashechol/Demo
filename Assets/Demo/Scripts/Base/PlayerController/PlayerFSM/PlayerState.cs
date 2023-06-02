using System.Collections;
using System.Collections.Generic;
using Demo.CombatSystem;
using Demo.Framework.FSM;
using Demo.Framework.Input;
using Demo.Framework.Gameplay;

namespace Demo.Base.PlayerController
{
    public class PlayerState : State
    {
        protected readonly PlayerController _player;
        protected readonly InputHandler _input;
        protected readonly Character _character;
        protected readonly Combat _combat;

        protected PlayerState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine)
        {
            _player = player;
            _input = player.input;
            _character = player.character;
            _combat = player.combat;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_input.JumpInput && _character.TryJump())
                _stateMachine.ChangeState(_player.jumpState);
            
            _character.OnUpdate();
        }
    }
}
