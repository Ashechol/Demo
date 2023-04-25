using System;
using Demo.Utils;
using UnityEngine;
using Framework.Camera;
using Inputs;
using Utils;

[RequireComponent(typeof(CameraHandler))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Components
    
    private CharacterController _controller;
    private InputHandler _input;
    private CameraHandler _camera;
    private PlayerAnim _anim;

    #endregion

    #region Locomotion
    
    [Header("Movement")]
    public float runSpeed = 6;
    public float accelerateTime = 0;
    public float angularTime = 0.5f;

    [Header("Jump")] 
    public float gravity = 20;

    #endregion
    
    /// Local horizontal direction
    private Vector3 _forward;
    private float _directionAngle;
    private float _forwardAngle;
    // Local velocity
    private Vector3 _motion;
    private float _motionY;
    private Vector3 _rotation;
    
    private float _curSpeed;
    
    public float CurSpeed => _curSpeed;
    
    private void Awake()
    {
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
        _anim = this.GetComponentSafe<PlayerAnim>();
    }

    private void Start()
    {
        _forward = transform.forward;
        _forwardAngle = 0;
    }

    private void Update()
    {
        Locomotion();
        _anim.UpdateAnimParams();
    }
    
    private void Locomotion()
    {
        Rotation();

        if (_controller.isGrounded)
            _motionY = -2;
        else
            _motionY -= gravity * Time.deltaTime;
        
        _curSpeed = Mathf.Lerp(_curSpeed, _input.IsMoveInput ? runSpeed * _input.MoveInput.magnitude : 0, accelerateTime * Time.deltaTime);
        if (_curSpeed < 0.1f) _curSpeed = 0;
        
        _motion = Quaternion.AngleAxis(_directionAngle, transform.up) * Vector3.forward * (_curSpeed * Time.deltaTime);

        _motion.y = _motionY * Time.deltaTime;
            
        _controller.Move(_motion);
    }
    
    
    [NonSerialized] private float _refAngle;
    private void Rotation()
    {
        if (_input.IsMoveInput)
        {
            _directionAngle = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.Yaw;
            _forwardAngle = Mathf.SmoothDampAngle(_forwardAngle,  _directionAngle, ref _refAngle, angularTime);
            _forwardAngle = Functions.ClampAngle(_forwardAngle, float.MinValue, float.MaxValue);
            _forward = Quaternion.Euler(0, _forwardAngle, 0) * Vector3.forward;
        }

        transform.forward = _forward;
    }
}
