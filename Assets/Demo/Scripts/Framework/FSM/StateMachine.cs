using System.Collections.Generic;
using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Framework.FSM
{
    public class StateMachine
    {
        private State _currentState;
        
#if UNITY_EDITOR
        private static readonly DebugLabel DebugLabel = new("StateMachine", Color.magenta);
#endif

        public virtual void Init(State defaultState)
        {
            _currentState = defaultState;
        }

        public void ChangeState(State nextState)
        {
            _currentState?.Exit();
            _currentState = nextState;
            _currentState?.Enter();
        }

        public void LogicUpdate()
        {
            _currentState?.LogicUpdate();
        }

        public void PhysicsState()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}
