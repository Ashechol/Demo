using System;
using UnityEngine;
using Demo.Utils;
using Demo.Utils.Debug;

namespace Demo.Base.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterBase : MonoBehaviour
    {
        internal AnimHandler animHandler;
        protected CharacterStateMachine stateMachine;
        protected Detection _detection;
        private DebugLabel _debugLabel;

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

        protected virtual void Awake()
        {
            this.GetComponentSafe<AnimHandler>();
            _detection = GetComponentInChildren<Detection>();
            _debugLabel = new DebugLabel(gameObject.name, Color.yellow);

#if UNITY_EDITOR
            if (!_detection)
                DebugLog.LabelLog(_debugLabel, "Missing Detection Component In Children!", Verbose.Error);
#endif

            stateMachine = new CharacterStateMachine(this);
        }

        protected void Start()
        {
            // stateMachine.Init();
        }

        protected void FixedUpdate()
        {
            stateMachine.PhysicsState();
        }
            
        protected void Update()
        {
            stateMachine.LogicUpdate();
        }
    }
}
