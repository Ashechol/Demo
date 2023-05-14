using Demo.Framework.Animation;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        private CharacterController _controller;
        private Detection _detection;
        [HideInInspector] public AnimController anim;

        [Header("Movement")] 
        public float walkSpeed = 1.8f;
        public float runSpeed = 5;
        public float dashSpeed = 12;
        public float acceleration = 15;
        public float angularTime = 0.1f;
        public float ledgeStuckAvoidForce = 0.5f;
        
        [Header("Jump And Fall")]
        public float gravity = 20;
        public float jumpHeight = 1.8f;
        
        private bool _isJump;

        private float _curSpeed;
        private float _targetYaw;

        private float _smoothYaw;
        
        private Vector3 _motion;
        private float _prevSpeedY;
        private float _fallSpeed;
        private Vector3 _rotation;

        private float _targetSpeed;
        private float _smoothAngle;

        public float CurSpeed => _curSpeed;
        public Vector3 Velocity => _controller.velocity;
        
        #region Mono Events

        protected void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _detection = GetComponentInChildren<Detection>();
            anim = GetComponent<AnimController>();
        }

        protected void Start()
        {
            _prevSpeedY = _controller.velocity.y;
        }
        
        #endregion

        /// Move character in with angle and speed.
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
            _controller.Move(_motion * Time.deltaTime);
        }
    
        private float _rotationSpeedRef;
        public float RotationSpeedRef => _rotationSpeedRef;
        public virtual void Turn(float targetAngle)
        {
            _targetYaw = targetAngle;
            _smoothAngle = Mathf.SmoothDampAngle(_smoothAngle, _targetYaw, ref _rotationSpeedRef, angularTime);
            transform.forward = Quaternion.AngleAxis(_smoothAngle, Vector3.up) * Vector3.forward;
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
            if (!_detection.IsGrounded && Velocity.y < 0.2f)
            {
                _fallSpeed = _prevSpeedY;
                _prevSpeedY = Velocity.y;
            }
        }

        private void AnimUpdate()
        {
            
        }
    }
}
