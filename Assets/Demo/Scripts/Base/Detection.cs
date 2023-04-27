using UnityEngine;

namespace Demo.Base
{
    public class Detection : MonoBehaviour
    {
        [Header("Ground Check")] 
        public float checkRadius;
        public LayerMask layerMask;
        public bool drawGizmos;

        public bool IsGrounded { get; private set; }

        private void Update()
        {
            IsGrounded = Physics.CheckSphere(transform.position, checkRadius, layerMask);
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
        
            Gizmos.color = IsGrounded ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, checkRadius);
        }
    }
}
