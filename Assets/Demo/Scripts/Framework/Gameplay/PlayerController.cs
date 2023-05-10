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
        }

        private void Binding()
        {
            _input.OnMovePerformed += Move;
            _input.OnLook += Look;
            _input.OnJump += Jump;
        }

        private void Move()
        {
            var speed = _character.runSpeed;
            if (_input.DashInput) speed = _character.dashSpeed;

            var targetAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.yaw;
            
            _character.MoveXZ(targetAngle , speed);
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
