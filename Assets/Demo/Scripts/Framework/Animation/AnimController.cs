using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Gameplay;

namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent))]
    public class AnimController : MonoBehaviour
    {
        private AnimancerComponent _anim;
        private Character _character;

        [SerializeField] private ClipTransition[] _idle;
        [SerializeField] private Float2ControllerTransition _movement;
        
        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;


        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();
        }

        private void OnEnable()
        {
            
        }

        public void UpdateParam()
        {
            if (_anim.IsPlaying(_movement.Key))
            {
                _movement.State.ParameterX = _character.CurSpeed;
                _movement.State.ParameterY = _character.RotationSpeedRef / _leanNormalizeAmount;
            }
        }

        public void PlayMove()
        {
            if (_anim.States.Current != _movement.State)
                _anim.Play(_movement);
        }

        public void PlayIdle() => _anim.Play(_idle[0]);
    }
}
