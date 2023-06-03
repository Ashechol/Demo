using System.Collections.Generic;
using Demo.Framework.Debug;
using UnityEngine;

namespace Demo.Framework.FSM
{
    public class StateMachine
    {
        private State _currentState;
        public State CurrentState => _currentState;

#if UNITY_EDITOR
        protected static readonly DebugLabel debugLabel = new("StateMachine", Color.magenta);
#endif

        public virtual void Init(State defaultState)
        {
#if UNITY_EDITOR
            if (defaultState == null)
                DebugLog.LabelLog(debugLabel, "Default State Is Null, Please Assign a State!", Verbose.Error);
#endif
                
            _currentState = defaultState;
            _currentState?.Enter();
        }

        public void ChangeState(State nextState)
        {
            _currentState?.Exit();
            _currentState = nextState;
            _currentState?.Enter();
        }

        public void ResetState()
        {
            _currentState.Enter();
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
