using System.Collections.Generic;
using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Framework.FSM
{
    public class StateMachineBase
    {
        private StateBase _currentState;
        
#if UNITY_EDITOR
        private static readonly DebugLabel DebugLabel = new("StateMachine", Color.magenta);
#endif

        public void Init(StateBase defaultState)
        {
            _currentState = defaultState;
        }

        public void ChangeState(StateBase nextState)
        {
            _currentState?.Exit();
            _currentState = nextState;
            _currentState?.Enter();
        }

        public void LogicUpdate()
        {
#if UNITY_EDITOR
            DebugLog.LabelLog(DebugLabel, 
                      "StateMachine Must Be Initialize First !\nUse Init()", 
                              Verbose.Assert, _currentState == null);
#endif
            
            _currentState?.LogicUpdate();
        }

        public void PhysicsState()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}
