using System;
using UnityEngine;
using Animancer;
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

        private ClipTransition[] _idles;
        private MixerTransition2D _move;
        private ClipTransition _jumpStart;
        private LinearMixerTransition _airBorne;
        private ClipTransitionSequence _landing;

        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;


        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
            _idles = _holder.idles;
            _move = _holder.move;
            _jumpStart = _holder.jumpStart;
            _airBorne = _holder.airBorne;
            _landing = _holder.landing;
        }

        public void PlayIdle(int index = 0) => _anim.Play(_idles[index]);
        public void PlayMove() => _anim.Play(_move);
        public void PlayJump() => _anim.Play(_jumpStart).Events.OnEnd += PlayAirBorne;
        public void PlayAirBorne() => _anim.Play(_airBorne);
        public void PlayLanding() => _anim.Play(_landing);
    }
}
