using UnityEngine;


namespace Demo.Framework.FSM
{
    public class State
    {
        protected StateMachine _stateMachine;

        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Enter() { }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Exit() { }
        
        public virtual void LogicUpdate() { }
        
        public virtual void PhysicsUpdate() { }
    }
}
