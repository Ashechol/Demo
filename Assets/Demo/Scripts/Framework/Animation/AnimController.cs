using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Debug;
using Demo.Framework.Utils;
using DG.Tweening;

namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent))]
    public class AnimController : MonoBehaviour
    {
        private AnimancerComponent _anim;
        [SerializeField] private AnimHolder _holder;
        private readonly DebugLabel _dbLabel = new DebugLabel("AnimController");

        private ClipTransition[] _idles;
        private MixerTransition2D _move;
        private ClipTransition[] _jump;
        private LinearMixerTransition _airBorne;
        private LinearMixerTransition _landing;

        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;
        
        /// <param name="exitTime"></param>
        /// <returns></returns>
        public bool IsAnimExiting(float exitTime) => _anim.States.Current.Time >= exitTime;
        
        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
            _idles = _holder.idles;
            _move = _holder.move;
            _jump = _holder.jump;
            _airBorne = _holder.airBorne;
            _landing = _holder.landing;
            
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
