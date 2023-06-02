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
        }

        # region Basic Motions
        
        public void PlayIdle(int index = 0) => _base.Play(_holder.idles[index]);
        public void PlayMove() => _base.Play(_holder.move);
        public void PlayJump() => _base.Play(_holder.jump[0]);
        public void PlaySecondJump() => _base.Play(_holder.jump[1]);
        public void PlayAirBorne() => _base.Play(_holder.airBorne);
        public void PlayLanding() => _base.Play(_holder.landing);
        public void PlayRunToStand() => _base.Play(_holder.runToStand);

        /// <param name="index">0: slide loop</param>
        /// <param name="index">1: slide to stand</param>
        public void PlayDashToStand(int index = 0) => _anim.Play(_holder.dashToStand[index]);
        
        # endregion

        #region Battle Motions

        /// <param name="index">0: Stand draw</param>
        /// <param name="index">1: Walk draw</param>
        public void PlayDrawWeapon(int index)
        {
            _holder.drawWeapon[index].Speed = -1;
            _base.Play(_holder.drawWeapon[index]);
        }

        /// <param name="index">0: Stand sheath</param>
        /// <param name="index">1: Walk sheath</param>
        public void PlaySheathWeapon(int index)
        {
            _holder.drawWeapon[index].Speed = 1;
            _base.Play(_holder.drawWeapon[index]);
        }

        public void PlayIdleWeapon(int index = 0) => _base.Play(_holder.idlesWeapon[index]);

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
            var moveState = _anim.States[_holder.move.Key] as MixerState<Vector2>;
            _leanAmount = rotationSpeedRef / _leanNormalizeAmount;

            if (moveState != null) 
                moveState.Parameter = new Vector2(speed, _leanAmount);
            else
                DebugLog.LabelLog(_dbLabel, "Missing move state!", Verbose.Warning);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateAirBorneParam(float speedY) => _holder.airBorne.State.Parameter = speedY;

        public void UpdateLandingParam(float speedY) => _holder.landing.State.Parameter = speedY;

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
