using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Debug;
using Demo.Framework.Gameplay;
using DG.Tweening;
using UnityEngine.Serialization;

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

            _jumpStart.Events.OnEnd += PlayAirBorne;
        }

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnGUIStats);
        }

        public void PlayIdle(int index = 0) => _anim.Play(_idles[index]);
        public void PlayMove() => _anim.Play(_move);
        public void PlayJump() => _anim.Play(_jumpStart);
        public void PlayAirBorne() => _anim.Play(_airBorne);
        public void PlayLanding() => _anim.Play(_landing);

        public void UpdateMoveParam(float speed, float rotationSpeedRef)
        {
            var moveState = _anim.States[_move.Key] as MixerState<Vector2>;
            _leanAmount = rotationSpeedRef / _leanNormalizeAmount;

            if (moveState != null) 
                moveState.Parameter = new Vector2(speed, _leanAmount);
            else
                DebugLog.LabelLog(_dbLabel, "Missing move state!", Verbose.Warning);
        }

        public void UpdateAirBorneParam(float speedY)
        {
            var airState = _anim.States[_airBorne.Key] as LinearMixerState;

            if (airState != null)
                airState.Parameter = speedY;
        }

        public void UpdateLandingParam(float speedY)
        {
            _landing.State.Parameter = speedY;
            
            // _landing.State.Norm
            
        }

        private void OnGUIStats()
        {
            var style = new GUIStyle
            {
                fontSize = 30
            };
            
            GUILayout.Label($"<color=yellow>Current Lean Amount: {_leanAmount}</color>", style);
        }
    }
}
