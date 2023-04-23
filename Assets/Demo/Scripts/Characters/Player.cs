using System;
using Framework.Camera;
using Inputs;
using UnityEngine;
using Utils;
using Utils.Log;

[RequireComponent(typeof(CameraHandler))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Components
    
    private CharacterController _controller;
    private InputHandler _input;
    private CameraHandler _camera;
    private Animator _anim;

    #endregion

    #region Locomotion
    
    [Header("Movement")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    // public float angularSpeed = 800;
    public float angularTime = 0.5f;

    [Header("Jump")] 
    public float gravity = 20;

    #endregion
    
    /// Local horizontal direction
    private Vector3 _forward;
    private float _directionAngle;
    public float _forwardAngle;
    // Local velocity
    private Vector3 _velocity;
    private float _curSpeed;
    private float _velocityY;
    private Vector3 _rotation;

    public float Speed => _curSpeed;
    
    private static readonly int SpeedXZ = Animator.StringToHash("speedXZ");

    private void Awake()
    {
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
        
        if (!transform.GetChild(0).TryGetComponent<Animator>(out _anim))
            DebugLog.Tips("Missing animator in children!", Color.white, Verbose.Warning);
    }

    private void Start()
    {
        _forward = transform.forward;
        _forwardAngle = 0;
    }

    private void Update()
    {
        Locomotion();

        if (_anim)
        {
            _anim.SetFloat(SpeedXZ, Speed);
        }
    }
    
    private void Locomotion()
    {
        Rotation();

        if (_controller.isGrounded)
            _velocityY = -2;
        else
            _velocityY -= gravity * Time.deltaTime;

        if (_input.IsMoveInput)
        {
            _velocity = Quaternion.AngleAxis(_directionAngle, transform.up) * Vector3.forward * (runSpeed * Time.deltaTime);
            _curSpeed = runSpeed;
        }
        else
        {
            _velocity = Vector3.zero;
            _curSpeed = 0;
        }

        _velocity.y = _velocityY * Time.deltaTime;
            
        _controller.Move(_velocity);
    }
    
    
    [NonSerialized] private float _refAngle;
    private void Rotation()
    {
        if (_input.IsMoveInput)
        {
            _directionAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.Yaw;
            _forwardAngle = Mathf.SmoothDampAngle(_forwardAngle,  _directionAngle, ref _refAngle, angularTime);
            _forward = Quaternion.Euler(0, _forwardAngle, 0) * Vector3.forward;
        }

        transform.forward = _forward;
    }
}
