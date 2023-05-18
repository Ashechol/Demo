using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Debug;
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
        private ClipTransition _jumpStart;
        private LinearMixerTransition _airBorne;
        private LinearMixerTransition _landing;
        private MixerTransition2D _landing2D;

        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;
        
        public bool IsAnimStopped => _anim.States.Current.IsStopped;
        
        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
            _idles = _holder.idles;
            _move = _holder.move;
            _jumpStart = _holder.jumpStart;
            _airBorne = _holder.airBorne;
            _landing = _holder.landing;
            _landing2D = _holder.landing2D;

            _jumpStart.Events.OnEnd += PlayAirBorne;
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
        public void PlayJump() => _anim.Play(_jumpStart);
        public void PlayAirBorne() => _anim.Play(_airBorne);
        public void PlayLanding() => _anim.Play(_landing2D);

        public void SetLandingEndEvent(Action callback) => _landing2D.Events.OnEnd += callback;
        
        /// 根运动速度
        public float AnimDesiredSpeed => _animDesiredSpeed;

        public void UpdateMoveParam(float speed, float rotationSpeedRef)
        {
            var moveState = _anim.States[_move.Key] as MixerState<Vector2>;
            _leanAmount = rotationSpeedRef / _leanNormalizeAmount;

            if (moveState != null) 
                moveState.Parameter = new Vector2(speed, _leanAmount);
            else
                DebugLog.LabelLog(_dbLabel, "Missing move state!", Verbose.Warning);
        }

        public void UpdateAirBorneParam(float speedY) => _airBorne.State.Parameter = speedY;

        public void UpdateLandingParam(float speedXZ, float speedY) => _landing2D.State.Parameter = new Vector2(speedXZ, speedY);

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
