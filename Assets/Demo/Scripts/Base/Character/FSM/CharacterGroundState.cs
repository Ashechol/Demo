namespace Demo.Base.Character
{
    public abstract class CharacterGroundState : CharacterState
    {
        protected CharacterGroundState(CharacterStateMachine stateMachine, string animTriggerName) 
            : base(stateMachine, animTriggerName)
        {
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!_character.IsGrounded && _character.VelocityY < 0)
                _stateMachine.ChangeState(_character.FallState);
        }
    }
}
