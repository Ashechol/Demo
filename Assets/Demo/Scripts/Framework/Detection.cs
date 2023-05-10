using System;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class Detection : MonoBehaviour
    {
        private Transform _prevTransform;
        private Transform _currentTransform;
        
        [Header("Ground Check")] 
        public float checkRadius;
        public float ledgeGroundCheckDistance;
        public LayerMask layerMask;
        public bool drawGizmos;

        private readonly RaycastHit[] _hits = new RaycastHit[4];
        private bool _isHitGround;
        [NonSerialized] private Vector3[] _rayCastDirection = Array.Empty<Vector3>();

        public bool IsGrounded { get; private set; }
        public bool IsHitGround 
        {
            get
            {
                var result = _isHitGround;
                _isHitGround = false;
                return result;
            }
        }
        public RaycastHit[] Hits => _hits;
        public bool IsLedgeStuck { get; private set; }

        private void Start()
        {
            _prevTransform = transform;
            _isHitGround = false;
        }

        private void Update()
        {
            _currentTransform = transform;
            IsLedgeStuck = _currentTransform.position == _prevTransform.position && !IsGrounded;
            _prevTransform = _currentTransform;
            
            var forward = _currentTransform.forward;
            var rightward = _currentTransform.right;
            
            _rayCastDirection = new[]{ forward, -forward, -rightward, rightward };

            GroundCheck();
        }
        
        private void GroundCheck()
        {
            var groundCheck = Physics.CheckSphere(_currentTransform.position, checkRadius, layerMask);
            if (groundCheck && !IsGrounded) _isHitGround = true;
            IsGrounded = groundCheck;

            for (var i = 0; i < 4; ++i)
                Physics.Raycast(_currentTransform.position, _rayCastDirection[i], out _hits[i], ledgeGroundCheckDistance, layerMask);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            var trans = transform;
            var position = trans.position;
            
            Gizmos.color = IsGrounded ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(position, checkRadius);

            var directions = Array.Empty<Vector3>();
            if (Application.isEditor)
            {
                var right = trans.right;
                var forward = trans.forward;
                directions = new[] { forward, -forward, -right, right };
            }

            for (var i = 0; i < 4; ++i) 
            {
                Gizmos.color = _hits[i].collider ? Color.green : Color.red;
                var dir = _rayCastDirection.Length == 4 ? _rayCastDirection[i] : directions[i];
                Gizmos.DrawLine(position, position + dir * ledgeGroundCheckDistance);
            }
        }
    }
}
