using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Debug;
using Demo.Framework.Utils;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent))]
    public class AnimController : MonoBehaviour
    {
        private AnimancerComponent _anim;
        
        [SerializeField] private AnimHolder _unarmedAnimations;
        [SerializeField] private AnimHolder _greatSwordAnimations;
        
        private readonly DebugLabel _dbLabel = new DebugLabel("AnimController");

        private ClipTransition[] _idles;
        private MixerTransition2D _move;
        private ClipTransition _runToStand;
        private ClipTransition[] _dashToStand;
        private ClipTransition[] _jump;
        private LinearMixerTransition _airBorne;
        private LinearMixerTransition _landing;

        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// Check whether current animation is in exiting transition.
        /// <param name="exitTime">Set exit time or use state normalized end time</param>
        public bool IsAnimExiting(float exitTime = -1)
        {
            if (exitTime < 0)
                return _anim.States.Current.NormalizedTime >= _anim.States.Current.NormalizedEndTime;

            return _anim.States.Current.Time >= exitTime;
        }

        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
            _idles = _unarmedAnimations.idles;
            _move = _unarmedAnimations.move;
            _jump = _unarmedAnimations.jump;
            _airBorne = _unarmedAnimations.airBorne;
            _landing = _unarmedAnimations.landing;
            _runToStand = _unarmedAnimations.runToStand;
            _dashToStand = _unarmedAnimations.dashToStand;

            // 对全部动画开启 foot ik
            // 只有 ClipState 是默认开启了的
            _anim.Playable.ApplyFootIK = true;
        }

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnGUIStats);
        }

        private float _animDesiredSpeed;
        private void OnAnimatorMove()
        {
            _animDesiredSpeed = _anim.Animator.velocity.magnitude / _anim.Animator.humanScale;
        }

        public void PlayIdle(int index = 0) => _anim.Play(_idles[index]);
        public void PlayMove() => _anim.Play(_move);
        public void PlayJump() => _anim.Play(_jump[0]);
        public void PlaySecondJump() => _anim.Play(_jump[1]);
        public void PlayAirBorne() => _anim.Play(_airBorne);
        public void PlayLanding() => _anim.Play(_landing);
        public void PlayRunToStand() => _anim.Play(_runToStand);

        /// <param name="index">0: slide loop</param>
        /// <param name="index">1: slide to stand</param>
        public void PlayDashToStand(int index = 0) => _anim.Play(_dashToStand[index]);


        /// 根运动速度
        public float AnimDesiredSpeed => _animDesiredSpeed;

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMoveParam(float speed, float rotationSpeedRef)
        {
            var moveState = _anim.States[_move.Key] as MixerState<Vector2>;
            _leanAmount = rotationSpeedRef / _leanNormalizeAmount;

            if (moveState != null) 
                moveState.Parameter = new Vector2(speed, _leanAmount);
            else
                DebugLog.LabelLog(_dbLabel, "Missing move state!", Verbose.Warning);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateAirBorneParam(float speedY) => _airBorne.State.Parameter = speedY;

        public void UpdateLandingParam(float speedY) => _landing.State.Parameter = speedY;

        private void OnGUIStats()
        {
            var style = new GUIStyle
            {
                fontSize = 30
            };
            
            GUILayout.Label($"<color=yellow>Current Lean Amount: {_leanAmount}</color>", style);
            GUILayout.Label($"<color=yellow>Anim Desired Speed: {AnimDesiredSpeed}</color>", style);
        }
    }
}
