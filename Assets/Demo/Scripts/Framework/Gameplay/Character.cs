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
            _detection = GetComponentInChildren<Detection>();
        }

        protected void Start()
        {
            _prevSpeedY = _movement.velocity.y;
            _forward = transform.forward;
        }

        protected void Update()
        {
            Fall();
            
            _movement.Move(_motion * Time.deltaTime);
            _velocity = _movement.velocity;
            transform.forward = _forward;
        }

        #endregion

        /// Move character in with angle and speed.
        /// <param name="targetAngle"> Directional angle in world space </param>
        /// <param name="speed"> Target move speed </param>
        public virtual void Move(float speed)
        {
            _targetSpeed = speed;
            _curSpeed = Mathf.Lerp(_curSpeed, _targetSpeed, acceleration * Time.deltaTime);
            
            // 因为 forward 旋转有时间，所以不能用 forward 作为移动方向
            // _motion.x = _forward.x * _curSpeed;
            // _motion.z = _forward.z * _curSpeed;

            var direction = Quaternion.AngleAxis(_targetYaw, transform.up) * Vector3.forward;
            _motion.x = direction.x * _curSpeed;
            _motion.z = direction.z * _curSpeed;
        }
    
        private float _rotationSpeedRef;
        public virtual void Turn(float targetAngle)
        {
            _targetYaw = targetAngle;
            _smoothAngle = Mathf.SmoothDampAngle(_smoothAngle, _targetYaw, ref _rotationSpeedRef, angularTime);
            _forward = Quaternion.Euler(0, _smoothAngle, 0) * Vector3.forward;
        }

        public virtual void Jump()
        {
            // 跳跃
            if (_detection.IsGrounded)
            {
                _motion.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                _isJump = true;
                _fallSpeed = 0.0f;
            }
        }

        private void Fall()
        {
            // 重力模拟
            if (_detection.IsGrounded && _motion.y < 0)
            {
                _motion.y = -2;
                // _movement.stepOffset = _stepOffset;
            }
            else
                _motion.y -= gravity * Time.deltaTime;
            
            // 下落速度记录
            if (!_detection.IsGrounded && _velocity.y < 0.2f)
            {
                _fallSpeed = _prevSpeedY;
                _prevSpeedY = _velocity.y;
            }
        }
    }
}
