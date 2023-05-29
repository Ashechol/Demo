using System.Collections;
using System.Collections.Generic;
using Demo.Framework.Debug;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public enum PlayerTransitionType
    {
        RunToStand,
        DashToStand,
        Default,
    }
    
    public class PlayerTransitionState : PlayerState
    {
        private PlayerTransitionType _type;
        private bool _hasType;

        private int _dashToStandIndex;

        private readonly DebugLabel _dbLabel = new("PlayerTransitionState");

        public PlayerTransitionState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
        {
        }

        public PlayerTransitionState SetType(PlayerTransitionType type)
        {
            _type = type;
            _hasType = true;
            return this;
        }

        public override void Enter()
        {
            base.Enter();

            if (!_hasType)
            {
                DebugLog.LabelLog(_dbLabel, "Need to set a player transition type!", Verbose.Warning);
                _type = PlayerTransitionType.Default;
            }
            
            switch (_type)
            {
                case PlayerTransitionType.RunToStand:
                    _character.anim.PlayRunToStand();
                    break;
                
                case PlayerTransitionType.DashToStand:
                    _dashToStandIndex = 0;
                    _character.anim.PlayDashToStand(_dashToStandIndex);
                    break;
                
                case PlayerTransitionType.Default:
                    break;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            switch (_type)
            {
                case PlayerTransitionType.RunToStand:
                    if (_character.anim.IsAnimExiting())
                        _stateMachine.ChangeState(_player.idleState);
                    if (_input.IsMoveInput)
                        _stateMachine.ChangeState(_player.moveState);
                    break;
                
                case PlayerTransitionType.DashToStand:
                    DashToStand();
                    break;
                
                case PlayerTransitionType.Default:
                    _stateMachine.ChangeState(_player.idleState);
                    break;
            }
        }

        private void DashToStand()
        {
            if (_dashToStandIndex == 0 && _character.anim.IsAnimExiting(_character.dashStopTime))
            {
                _character.SetTargetSpeed(0);
                _character.anim.PlayDashToStand(++_dashToStandIndex);
            }
            else if (_dashToStandIndex == 1 && _character.anim.IsAnimExiting())
            {
                _stateMachine.ChangeState(_player.idleState);
            }
            else if (_dashToStandIndex == 0)
                _character.SetTargetSpeed(_character.dashStopSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            _hasType = false;
        }
    }
}
