using System;
using UnityEngine;
using Demo.Utils;
using Demo.Utils.Debug;
using UnityEngine.EventSystems;

namespace Demo.Base.Character
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterBase : MonoBehaviour
    {
        internal AnimHandler _animHandler;
        protected CharacterStateMachine stateMachine;
        protected Detection _detection;
        protected CharacterController _controller;
        private DebugLabel _debugLabel;

        #region States

        public CharacterState DefaultState { get; protected set; }
        public CharacterIdleState IdleState { get; protected set; }
        public CharacterMoveState MoveState { get; protected set; }
        public CharacterFallState FallState { get; protected set; }

        #endregion


        [Header("Movement")] 
        public float walkSpeed = 2;
        public float runSpeed = 6;
        public float dashSpeed = 8;
        public float acceleration = 15;
        public float angularTime = 0.5f;
        public float ledgeStuckAvoidForce = 0.5f;

        [Header("Jump")] 
        public float gravity = 20;
        public float jumpHeight = 1.8f;

        #region InternalStats
        
        private bool _isJump;
        private bool _canJump = true;
        private float _landingRecoverySpeed;
        
        /// Local horizontal direction
        private Vector3 _forward;

        private float _curSpeed;
        private float _targetYaw;

        private float _smoothYaw;

        // Local velocity
        private float _desiredSpeed;
        private Vector3 _motion;
        private float _motionY;
        private Vector3 _velocity;
        private float _prevSpeedY;
        private float _fallSpeed;
        private Vector3 _rotation;

        #endregion
        
        #region Stats
        public float CurSpeed => _velocity.MagnitudeXZ();
        public float VelocityY => _controller.velocity.y;
        public bool IsGrounded => _detection.IsGrounded;
        public float FallSpeed => -_fallSpeed;
        
        public bool IsJump
        {
            get
            {
                var result = _isJump;
                _isJump = false;
                return result;
            }
        }
        
        #endregion
        
        /// Assign basic state.
        /// You can do nothing and let the CharacterBase create states by default.
        protected abstract void InitState();
        
        protected virtual void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _animHandler = this.GetComponentSafe<AnimHandler>();
            _detection = GetComponentInChildren<Detection>();
            _debugLabel = new DebugLabel(gameObject.name, Color.yellow);

#if UNITY_EDITOR
            if (!_detection)
                DebugLog.LabelLog(_debugLabel, "Missing Detection Component In Children!", Verbose.Error);
#endif

            stateMachine = new CharacterStateMachine(this);
            InitState();

            IdleState ??= new CharacterIdleState(stateMachine, "idle");
            MoveState ??= new CharacterMoveState(stateMachine, "move");
            FallState ??= new CharacterFallState(stateMachine, "fall");
        }

        protected void Start()
        {
            // DefaultState = IdleState;
            stateMachine.Init(DefaultState);
            _desiredSpeed = runSpeed;
        }

        protected void FixedUpdate()
        {
            stateMachine.PhysicsState();
        }
            
        protected void Update()
        {
            stateMachine.LogicUpdate();
            _animHandler.UpdateAnimParams();
        }
        
        // private void Rotation()
        // {
        //     if (_input.IsMoveInput && _curSpeed > 0.1f)
        //     {
        //         _targetYaw = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.Yaw;
        //         _targetYaw = Functions.ClampAngle(_targetYaw, -360, 360);
        //         _smoothYaw = Mathf.SmoothDampAngle(_smoothYaw, _targetYaw, ref _rotationRef, angularTime);
        //         _forward = Quaternion.Euler(0, _smoothYaw, 0) * Vector3.forward;
        //     }
        //
        //     transform.forward = _forward;
        // }
        //
        // private void Locomotion(Vector3 moveInput)
        // {
        //     moveInput = Functions.NearlyEqual(_desiredSpeed, runSpeed, 0.01f) ? _input.MoveInput.magnitude : 1;
        //     var targetSpeed = _input.IsMoveInput ? _desiredSpeed * moveInput : 0;
        //     _curSpeed = Mathf.Lerp(_curSpeed, targetSpeed, acceleration * Time.deltaTime);
        //     if (_curSpeed < 0.1f) _curSpeed = 0;
        //
        //     _motion = Quaternion.AngleAxis(_targetYaw, transform.up) * Vector3.forward * (_curSpeed * Time.deltaTime);
        //
        //     _motion.y = _motionY * Time.deltaTime;
        //
        //     if (_detection.IsLedgeStuck)
        //         _motion += AvoidLedgeStuck() * Time.deltaTime;
        //
        //     _controller.Move(_motion);
        //     _velocity = _controller.velocity;
        // }
        
        private Vector3 AvoidLedgeStuck()
        {
            var avoidDirection = Vector3.zero;

            foreach (var hit in _detection.Hits)
            {
                if (!hit.collider) continue;

                avoidDirection += hit.normal;
            }

            return avoidDirection * ledgeStuckAvoidForce;
        }
    }
}
