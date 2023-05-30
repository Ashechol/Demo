using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(AnimancerComponent), typeof(Animator))]
    [ExecuteInEditMode]
    public class AnimLayerBlendPreviewer : MonoBehaviour
    {
        private AnimancerComponent _anim;
        private AnimancerLayer _layer0;
        private AnimancerLayer _layer1;

        public AvatarMask _mask0;
        public AvatarMask _mask1;

        public AnimationClip _clip0;
        public AnimationClip _clip1;

        public bool play;
        
        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();

            _layer0 = _anim.Layers[0];
            _layer1 = _anim.Layers[1];
            
            if (_mask0) _layer0.SetMask(_mask0);
            if (_mask1) _layer1.SetMask(_mask1);
        }

        private void Update()
        {
            if (play)
            {
                _layer0.Play(_clip0);
                _layer1.Play(_clip1);
            }
        }
    }
}
