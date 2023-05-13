using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using DG.Tweening;


namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent))]
    public class AnimController : MonoBehaviour
    {
        private AnimancerComponent _anim;

        [SerializeField] private ClipTransition[] _idle;
        
        [SerializeField] private LinearMixerTransition _walk;
        [SerializeField] private LinearMixerTransition _run;
        [SerializeField] private LinearMixerTransition _dash;

        public float speed;
        public float leanAmount;
        
        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
        }

        private void OnEnable()
        {
            _anim.Play(_idle[0]);
        }

        public void AnimMove()
        {
            var state = _run.State;
            
            switch (speed)
            {
                case <= 2f:
                    state = _anim.Play(_walk) as LinearMixerState;
                    break;
                case <= 5.3f:
                    state = _anim.Play(_run) as LinearMixerState;
                    break;
                case <= 12.1f:
                    state = _anim.Play(_dash) as LinearMixerState;
                    break;
            }

            if (state != null) state.Parameter = leanAmount;
        }

        public bool IsAnimMove => _anim.IsPlaying(_walk) || _anim.IsPlaying(_run) || _anim.IsPlaying(_dash);
    }
}
