using System;
using Demo.Framework.Camera;
using Demo.Framework.Debug;
using Demo.Framework.Input;
using Demo.Framework.Utils;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    [RequireComponent(typeof(CameraHandler))]
    public class PlayerController : MonoBehaviour
    {
        internal Character character;
        private CameraHandler _camera;
        internal InputHandler input;

        [Header("Camera Control Settings")]
        [SerializeField] private float _axisYSpeed = 5;
        [SerializeField] private float _axisXSpeed = 5;
        [SerializeField] private float _pitchMax = 60;
        [SerializeField] private float _pitchMin = -30;

        internal float cameraYaw;

        internal PlayerStateMachine stateMachine;
        internal PlayerIdleState idleState;
        internal PlayerMoveState moveState;
        internal PlayerJumpState jumpState;
        internal PlayerAirBorneState airBorneState;

        private void Awake()
        {
            character = GetComponentInChildren<Character>();
            input = this.GetComponentSafe<InputHandler>();
            _camera = GetComponent<CameraHandler>();

            stateMachine = new PlayerStateMachine();
            idleState = new PlayerIdleState(stateMachine, this);
            moveState = new PlayerMoveState(stateMachine, this);
            jumpState = new PlayerJumpState(stateMachine, this);
            airBorneState = new PlayerAirBorneState(stateMachine, this);
            
            // stateMachine.Init(idleState);
        }

        private void Start()
        {
            stateMachine.Init(idleState);
        }

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnGUIStats);
        }

        private void Update()
        {
            stateMachine.LogicUpdate();
        }

        private void LateUpdate()
        {
            Look();
            _camera.OnLateUpdate();
        }

        // private void Move()
        // {
        //     float targetSpeed = 0;
        //     
        //     if (input.IsMoveInput)
        //     {
        //         var targetAngle = Mathf.Atan2(input.MoveInputX, input.MoveInputY) * Mathf.Rad2Deg + _camera.yaw;
        //         character.Turn(targetAngle);
        //
        //         targetSpeed = character.runSpeed;
        //         if (input.DashInput) targetSpeed = character.dashSpeed;
        //     }
        //     
        //     character.Move(targetSpeed);
        // }

        private void Look()
        {
            _camera.yaw += input.YawInput * _axisXSpeed;
            _camera.pitch += input.PitchInput * _axisYSpeed;
            
            _camera.yaw = Functions.ClampAngle(_camera.yaw);
            _camera.pitch = Functions.ClampAngle(_camera.pitch, _pitchMin, _pitchMax);

            cameraYaw = _camera.yaw;
        }

        private void Jump()
        {
            if (input.JumpInput)
                character.Jump();
        }

        private void OnGUIStats()
        {
            var style = new GUIStyle
            {
                fontSize = 30
            };
            GUILayout.Label($"<color=yellow>Current State: {stateMachine.CurrentState.GetType().Name}</color>", style);
        }
    }
}
