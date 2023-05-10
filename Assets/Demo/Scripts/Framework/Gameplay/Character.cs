using UnityEngine;
using CharacterMovement = UnityEngine.CharacterController;

namespace Demo.Framework.Gameplay
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Character : MonoBehaviour
    {
        private CharacterMovement _movement;
        private PlayerController _controller;
        private Detection _detection;

        [Header("Movement")] 
        public float walkSpeed = 2;
        public float runSpeed = 6;
        public float dashSpeed = 10;
        public float acceleration = 15;
        public float angularTime = 0.1f;
        public float ledgeStuckAvoidForce = 0.5f;
        
        [Header("Jump And Fall")]
        public float gravity = 20;
        public float jumpHeight = 1.8f;
        
        private bool _isJump;

        /// Local horizontal direction
        private Vector3 _forward;

        private float _curSpeed;
        private float _targetYaw;

        private float _smoothYaw;
        
        private Vector3 _motion;
        private float _motionY;
        private Vector3 _velocity;
        private float _prevSpeedY;
        private float _fallSpeed;
        private Vector3 _rotation;

        private float _targetSpeed;
        private float _smoothAngle;
        
        #region Mono Events

        protected void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
            // _controller = 
        }

        protected void Start()
        {
            _prevSpeedY = _movement.velocity.y;
        }

        protected void Update()
        {
            Fall();
            
            _movement.Move(_motion * Time.deltaTime);
            _velocity = _movement.velocity;
        }

        #endregion

        private float _rotationSpeedRef;
        
        /// Move character in with angle and speed.
        /// <param name="targetAngle"> Directional angle in world space </param>
        /// <param name="speed"> Target move speed </param>
        public virtual void MoveXZ(float targetAngle, float speed)
        {
            _targetSpeed = speed;
            
            _curSpeed = Mathf.Lerp(_curSpeed, _targetSpeed, acceleration * Time.deltaTime);
            _smoothAngle = Mathf.SmoothDampAngle(_smoothAngle, targetAngle, ref _rotationSpeedRef, angularTime);
            var direction = Quaternion.Euler(0, _smoothAngle, 0) * Vector3.forward;
            
            _motion.x = direction.x * _curSpeed;
            _motion.z = direction.y * _curSpeed;
        }

        public virtual void Jump()
        {
            // 跳跃
            if (_detection.IsGrounded)
            {
                _motionY = Mathf.Sqrt(2 * jumpHeight * gravity);
                _isJump = true;
                _fallSpeed = 0.0f;
            }
        }

        private void Fall()
        {
            // 重力模拟
            if (_detection.IsGrounded && _motionY < 0)
            {
                _motionY = -2;
                // _movement.stepOffset = _stepOffset;
            }
            else
                _motionY -= gravity * Time.deltaTime;
            
            // 下落速度记录
            if (!_detection.IsGrounded && _velocity.y < 0.2f)
            {
                _fallSpeed = _prevSpeedY;
                _prevSpeedY = _velocity.y;
            }
        }
    }
}
