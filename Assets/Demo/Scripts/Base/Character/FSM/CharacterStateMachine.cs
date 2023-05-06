using Demo.Framework.FSM;

namespace Demo.Base.Character
{
    public class CharacterStateMachine : StateMachineBase
    {
        internal readonly CharacterBase character;
        
        public CharacterStateMachine(CharacterBase character)
        {
            this.character = character;
        }
    }
}
