namespace Demo.Base.Character
{
    public class CharacterMoveState : CharacterGroundState
    {
        public CharacterMoveState(CharacterStateMachine stateMachine, string animTriggerName) 
            : base(stateMachine, animTriggerName) {}
        
    }
}
