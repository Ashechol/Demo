namespace Demo.Base.Character
{
    public class CharacterIdleState : CharacterGroundState
    {
        public CharacterIdleState(CharacterStateMachine stateMachine, string animTriggerName) 
            : base(stateMachine, animTriggerName)
        {
        }
    }
}
