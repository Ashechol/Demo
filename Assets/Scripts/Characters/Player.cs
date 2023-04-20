using System;
using Framework.Camera;
using Inputs;
using UnityEngine;
using Utils;

[RequireComponent(typeof(CameraHandler))]
public class Player : MonoBehaviour
{
    #region Components
    
    private CharacterController _controller;
    private InputHandler _input;
    private CameraHandler _camera;
    
    #endregion
    
    /// Local horizontal direction
    private Vector3 _forward;
    private Vector3 _targetDirection;
    public float _forwardAngle;
    // Local velocity
    private Vector3 _velocity;
    private float _velocityY;
    private Vector3 _rotation;

    #region Locomotion
    
    [Header("Movement")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float angularSpeed = 800;
    public float angularTime = 0.5f;

    [Header("Jump")] 
    public float gravity = 20;

    #endregion

    private void Awake()
    {
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
    }

    private void Start()
    {
        _forward = transform.forward;
        _forwardAngle = 0;
    }

    private void Update()
    {
        Locomotion();
    }

    private float _refVelocity;
    private void Locomotion()
    {
        if (_input.MoveInput != Vector2.zero)
        {
            float directionAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg;
            _forwardAngle = Mathf.SmoothDampAngle(_forwardAngle, _camera.Yaw + directionAngle, ref _refVelocity,
                angularTime);
            _forward = Quaternion.Euler(0, _forwardAngle, 0) * Vector3.forward;
        }
        
        transform.forward = _forward;
    }

    private void Rotation()
    {
        
    }
}
