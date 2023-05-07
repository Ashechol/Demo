using Demo.Framework.FSM;
using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Base.Character
{
    public class CharacterState : State
    {
        private readonly string _animParam;
        private readonly AnimHandler _anim;
        
        #if UNITY_EDITOR
        private readonly DebugLabel _debugLabel;
        #endif
        
        public CharacterState(CharacterStateMachine stateMachine, string animTriggerName) : base(stateMachine)
        {
            _anim = stateMachine.character.animHandler;
            _animParam = animTriggerName;
        }

        public override void Enter()
        {
            base.Enter();
            
            _anim.SetTrigger(_animParam);
        }
    }
}
