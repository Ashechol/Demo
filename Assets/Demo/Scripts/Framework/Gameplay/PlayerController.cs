using System;
using Demo.Framework.Camera;
using Demo.Framework.Input;
using Demo.Framework.Utils;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    [RequireComponent(typeof(CameraHandler))]
    public class PlayerController : MonoBehaviour
    {
        private Character _character;
        private CameraHandler _camera;
        private InputHandler _input;
        
        [Header("Camera Control Settings")]
        [SerializeField] private float _axisYSpeed = 5;
        [SerializeField] private float _axisXSpeed = 5;
        [SerializeField] private float _pitchMax = 60;
        [SerializeField] private float _pitchMin = -30;

        private void Awake()
        {
            _character = GetComponentInChildren<Character>();
            _input = this.GetComponentSafe<InputHandler>();
            _camera = GetComponent<CameraHandler>();
        }

        private void Start()
        {
            Binding();
        }

        private void Update()
        {
            Move();
        }

        private void Binding()
        {
            _input.OnLook += Look;
            _input.OnJump += Jump;
        }

        private void Move()
        {
            float targetSpeed = 0;
            
            if (_input.IsMoveInput)
            {
                var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.yaw;
                _character.Turn(targetAngle);

                targetSpeed = _character.runSpeed;
                if (_input.DashInput) targetSpeed = _character.dashSpeed;
            }
            
            _character.Move(targetSpeed);
        }

        private void Look()
        {
            _camera.yaw += _input.YawInput * _axisXSpeed;
            _camera.pitch += _input.PitchInput * _axisYSpeed;
            
            _camera.yaw = Functions.ClampAngle(_camera.yaw);
            _camera.pitch = Functions.ClampAngle(_camera.pitch, _pitchMin, _pitchMax);
        }

        private void Jump()
        {
            if (_input.JumpInput)
                _character.Jump();
        }
    }
}
