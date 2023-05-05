using UnityEngine;


namespace Demo.Framework.FSM
{
    public class StateBase
    {
        private StateMachineBase _stateMachine;

        public StateBase(StateMachineBase stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public virtual void Enter() { }
        
        public virtual void Exit() { }
        
        public virtual void LogicUpdate() { }
        
        public virtual void PhysicsUpdate() { }
    }
}
