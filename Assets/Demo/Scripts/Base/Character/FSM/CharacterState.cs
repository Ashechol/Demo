using Demo.Framework.FSM;
using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Base.Character
{
    public class CharacterState : StateBase
    {
        private readonly int _animParam;
        private readonly Animator _anim;
        
        #if UNITY_EDITOR
        private readonly DebugLabel _debugLabel;
        #endif
        
        public CharacterState(CharacterStateMachine stateMachine, string animTriggerName) : base(stateMachine)
        {
            _anim = stateMachine.character.anim;
            _animParam = Animator.StringToHash(animTriggerName);
        }

        public override void Enter()
        {
            base.Enter();
            
            _anim.SetTrigger(_animParam);
        }
    }
}
