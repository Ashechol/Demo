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
        public float dashStopSpeed = 2;
        public float dashStopTime = 0.5f;
        public float acceleration = 15;
        public float angularTime = 0.1f;
        public float ledgeStuckAvoidForce = 0.5f;
        private Vector3 _moveDirection;
        
        [Header("Jump And Fall")]
        public float gravity = 20;
        public float airSpeed = 2.5f;
        public float jumpHeight = 1.8f;
        public float secondJumpHeight = 0.5f;

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
        public float FallSpeed => -_fallSpeed;
        public Vector3 Velocity => _controller.velocity;
        public bool IsGrounded => _detection.IsGrounded;
        
        #region Mono Events

        protected void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _detection = GetComponentInChildren<Detection>();
            anim = GetComponentInChildren<AnimController>();
        }

        protected void Start()
        {
            _prevSpeedY = _controller.velocity.y;
        }
        
        #endregion

        public void OnUpdate()
        {
            Fall();
            
            _curSpeed = Mathf.Lerp(_curSpeed, _targetSpeed, acceleration * Time.deltaTime);
            _motion.x = _moveDirection.x * _curSpeed;
            _motion.z = _moveDirection.z * _curSpeed;
            
            if (_detection.IsLedgeStuck)
                _motion += AvoidLedgeStuck();
            
            _controller.Move(_motion * Time.deltaTime);
        }

        /// Move character in with target speed.
        /// <param name="speed"> Target move speed </param>
        public virtual void SetTargetSpeed(float speed)
        {
            _targetSpeed = speed;

            // 因为 forward 旋转有时间，所以不能用 forward 作为移动方向
            // _motion.x = _forward.x * _curSpeed;
            // _motion.z = _forward.z * _curSpeed;
            
            if (speed > 0)
                _moveDirection = Quaternion.AngleAxis(_targetYaw, transform.up) * Vector3.forward;
        }
    
        private float _rotationSpeedRef;
        public float RotationSpeedRef => _rotationSpeedRef;
        public virtual void Turn(float targetAngle)
        {
            _targetYaw = targetAngle;
            _smoothAngle = Mathf.SmoothDampAngle(_smoothAngle, _targetYaw, ref _rotationSpeedRef, angularTime);
            transform.forward = Quaternion.AngleAxis(_smoothAngle, Vector3.up) * Vector3.forward;
        }


        private int _jumpTime;
        public virtual void Jump(float height)
        {
            // 跳跃
            _motion.y = Mathf.Sqrt(2 * height * gravity);
            _fallSpeed = 0.0f;
        }
        
        public virtual bool TryJump()
        {
            if (_detection.IsGrounded || _jumpTime < 2)
            {
                Jump(_jumpTime == 0 ? jumpHeight : secondJumpHeight);
                ++_jumpTime;
                return true;
            }
            return false;
        }

        private void Fall()
        {
            // 重力模拟
            if (_detection.IsGrounded && _motion.y < 0)
            {
                _motion.y = -2;
                _jumpTime = 0;
            }
            else
                _motion.y -= gravity * Time.deltaTime;
            
            // 下落速度记录
            if (!_detection.IsGrounded && Velocity.y < 0f)
            {
                _fallSpeed = _prevSpeedY;
                _prevSpeedY = Velocity.y;
            }
        }
        
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
