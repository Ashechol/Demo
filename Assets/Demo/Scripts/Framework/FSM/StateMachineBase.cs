using System.Collections.Generic;

namespace Demo.Framework.FSM
{
    public class StateMachineBase
    {
        private StateBase _currentState;

        public void ChangeState(StateBase nextState)
        {
            _currentState.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        public void LogicUpdate()
        {
            _currentState.LogicUpdate();
        }

        public void PhysicsState()
        {
            _currentState.PhysicsUpdate();
        }
    }
}
