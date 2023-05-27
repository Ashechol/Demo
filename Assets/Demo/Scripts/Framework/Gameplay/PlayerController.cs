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

        private PlayerStateMachine _stateMachine;
        internal PlayerIdleState idleState;
        internal PlayerMoveState moveState;
        internal PlayerJumpState jumpState;
        internal PlayerAirBorneState airBorneState;
        internal PlayerLandingState landingState;

        private void Awake()
        {
            character = GetComponentInChildren<Character>();
            input = this.GetComponentSafe<InputHandler>();
            _camera = GetComponent<CameraHandler>();

            _stateMachine = new PlayerStateMachine();
            idleState = new PlayerIdleState(_stateMachine, this);
            moveState = new PlayerMoveState(_stateMachine, this);
            jumpState = new PlayerJumpState(_stateMachine, this);
            airBorneState = new PlayerAirBorneState(_stateMachine, this);
            landingState = new PlayerLandingState(_stateMachine, this);

            // stateMachine.Init(idleState);
        }

        private void Start()
        {
            _stateMachine.Init(idleState);
        }

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnGUIStats);
        }

        private void Update()
        {
            _stateMachine.LogicUpdate();
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

        private void OnGUIStats()
        {
            var style = new GUIStyle
            {
                fontSize = 30
            };
            GUILayout.Label($"<color=yellow>Current State: {_stateMachine.CurrentState.GetType().Name}</color>", style);
            GUILayout.Label($"<color=yellow>Fall Speed: {character.FallSpeed}</color>", style);
            GUILayout.Label($"<color=yellow>Current Speed: {character.CurSpeed}</color>", style);
        }
    }
}
