using Demo.Framework.FSM;

namespace Demo.Base.Character
{
    public class CharacterStateMachine : StateMachine
    {
        internal readonly CharacterBase character;
        
        public CharacterStateMachine(CharacterBase character)
        {
            this.character = character;
        }
    }
}
