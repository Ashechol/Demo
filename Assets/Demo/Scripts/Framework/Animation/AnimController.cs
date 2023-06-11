using System;
using UnityEngine;
using Animancer;
using Demo.Framework.Debug;

namespace Demo.Framework.Animation
{
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent))]
    public class AnimController : MonoBehaviour
    {
        private AnimancerComponent _anim;
        
        [SerializeField] private AnimHolder _holder;
        [SerializeField] private AvatarMask _upperBodyMask;

        private AnimancerLayer _base;
        private AnimancerLayer _upperBody;

        private readonly DebugLabel _dbLabel = new("AnimController");

        [SerializeField] private float _leanNormalizeAmount = 300;
        private float _leanAmount;
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// Check whether current animation is in exiting transition.
        /// <param name="exitTime">Set exit time or use state normalized end time</param>
        public bool IsAnimExiting(float exitTime = -1, int index = 0)
        {
            var layer = index == 0 ? _base : _upperBody;

            if (exitTime < 0)
            {
                if (layer.CurrentState.Speed > 0)
                    return layer.CurrentState.NormalizedTime >= _anim.States.Current.NormalizedEndTime;
                
                return layer.CurrentState.NormalizedTime <= 0;
            }
            
            if (layer.CurrentState.Speed > 0)
                return layer.CurrentState.Time >= exitTime;
            
            return layer.CurrentState.Time <= layer.CurrentState.Length - exitTime;
        }

        private void Awake()
        {
            _anim = GetComponent<AnimancerComponent>();

            // 对全部动画开启 foot ik
            // 只有 ClipState 是默认开启了的
            _anim.Playable.ApplyFootIK = true;

            _base = _anim.Layers[0];
            _upperBody = _anim.Layers[1];
            _upperBody.SetMask(_upperBodyMask);
        }

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnGUIStats);

            AnimEventRegister();
        }

        private void AnimEventRegister()
        {
            _holder.drawWeapon[0].Events.SetCallback(0, OnWeaponDrawSheath);
            _holder.drawWeapon[1].Events.SetCallback(0, OnWeaponDrawSheath);
            _holder.drawWeapon[1].Events.OnEnd += () => FadeOutUpperBody(_holder.drawWeapon[1].FadeDuration);
        }

        # region Basic Motions

        public void PlayIdle(int index = 0, bool isWeaponDrawn = false)
        {
            _base.Play(isWeaponDrawn ? _holder.idlesWeapon[index] : _holder.idles[index]);
        }
        public void PlayMove(bool isWeaponDrawn = false) => _base.Play(isWeaponDrawn ? _holder.moveWeapon : _holder.move);
        public void PlayJump(bool isWeaponDrawn = false) => _base.Play(isWeaponDrawn ? _holder.jumpWeapon[0] : _holder.jump[0]);
        public void PlaySecondJump() => _base.Play(_holder.jump[1]);
        public void PlayAirBorne(bool isWeaponDrawn = false) => _base.Play(isWeaponDrawn ? _holder.airBorneWeapon : _holder.airBorne);
        public void PlayLanding(bool isWeaponDrawn = false) => _base.Play(isWeaponDrawn ? _holder.landingWeapon : _holder.landing);
        public void PlayRunToStand(bool isWeaponDrawn = false) => _base.Play(isWeaponDrawn ? _holder.runToStandWeapon : _holder.runToStand);

        /// <param name="index">0: slide loop</param>
        /// <param name="index">1: slide to stand</param>
        public void PlayDashToStand(int index = 0) => _anim.Play(_holder.dashToStand[index]);
        
        # endregion

        #region Battle Motions
        
        public void PlayDrawWeapon(bool isMoving = false)
        {
            if (!isMoving)
            {
                _holder.drawWeapon[0].Speed = -1;
                _base.Play(_holder.drawWeapon[0]);
            }
            else
            {
                _holder.drawWeapon[1].Speed = -1;
                _holder.drawWeapon[1].NormalizedStartTime = 1;
                _upperBody.Play(_holder.drawWeapon[1]);
            }
        }
        
        public void PlaySheathWeapon(bool isMoving = false)
        {
            if (!isMoving)
            {
                _holder.drawWeapon[0].Speed = 1;
                _base.Play(_holder.drawWeapon[0]);
            }
            else
            {
                _holder.drawWeapon[1].Speed = 1;
                _holder.drawWeapon[1].NormalizedStartTime = 0;
                _upperBody.Play(_holder.drawWeapon[1]);
            }
        }

        public void PlayComboAttack(int index) => _base.Play(_holder.comboAttacks[index]);

        public bool CanCombo()
        {
            
            
            return false;
        }

        #endregion
        
        #region Animation Events
        
        public Action onWeaponDrawSheath;
        public void OnWeaponDrawSheath() => onWeaponDrawSheath?.Invoke();
        
        #endregion
        
        private float _animDesiredSpeed;
        private void OnAnimatorMove()
        {
            _animDesiredSpeed = _anim.Animator.velocity.magnitude / _anim.Animator.humanScale;
        }
        
        /// 根运动速度
        public float AnimDesiredSpeed => _animDesiredSpeed;

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMoveParam(float speed, float rotationSpeedRef)
        {
            if (_base.CurrentState is MixerState<Vector2> moveState)
            {
                _leanAmount = rotationSpeedRef / _leanNormalizeAmount;
                moveState.Parameter = new Vector2(speed, _leanAmount);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateAirBorneParam(float speedY)
        {
            if (_base.CurrentState.Key != _holder.airBorneWeapon.Key &&
                _base.CurrentState.Key != _holder.airBorne.Key)
                return;
            
            if (_base.CurrentState is LinearMixerState state)
                state.Parameter = speedY;
        }

        public void UpdateLandingParam(float speedY)
        {
            if (_base.CurrentState.Key != _holder.landingWeapon.Key &&
                _base.CurrentState.Key != _holder.landing.Key)
                return;
            
            if (_base.CurrentState is LinearMixerState state)
                state.Parameter = speedY;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void FadeOutUpperBody(float duration) => _upperBody.StartFade(0, duration);

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
