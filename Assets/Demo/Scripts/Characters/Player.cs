using System;
using Demo.Base;
using Demo.Utils;
using UnityEngine;
using Framework.Camera;
using Inputs;
using UnityEngine.Serialization;
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
    private Detection _detection;

    #endregion
    
#if UNITY_EDITOR
    private readonly DebugLabel _debugLabel = new DebugLabel
    {
        labelColor = Color.yellow,
        messageColor = Color.white
    };
#endif

    #region Locomotion
    
    [Header("Movement")]
    public float runSpeed = 6;
    public float acceleration = 15;
    public float angularTime = 0.5f;
    public float ledgeStuckAvoidForce = 0.5f;

    [Header("Jump")] 
    public float gravity = 20;
    public float jumpHeight = 5;
    public float jumpTimeout = 0.2f;
    
    /// Local horizontal direction
    private Vector3 _forward;
    private float _curSpeed;
    private float _targetYaw;
    private float _smoothYaw;
    // Local velocity
    private Vector3 _motion;
    private float _motionY;
    private Vector3 _rotation;
    
    public float CurSpeed => _curSpeed;
    public bool IsJump { get; private set; }
    public float VelocityY => _controller.velocity.y;
    public bool IsGrounded => _detection.IsGrounded;

    #endregion

    private void Awake()
    {
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
        _anim = this.GetComponentSafe<PlayerAnim>();
        _detection = GetComponentInChildren<Detection>();
        
#if UNITY_EDITOR
        if(!_detection) 
            DebugLog.LabelLog(_debugLabel, "Missing Detection Component In Children!", Verbose.Error);
#endif
    }

    private void Start()
    {
        var trans = transform;
        _forward = trans.forward;
    }

    private void Update()
    {
        Locomotion();
        _anim.UpdateAnimParams();
    }
    
    private void Locomotion()
    {
        Rotation();

        JumpAndFall();
        
        _curSpeed = Mathf.Lerp(_curSpeed, _input.IsMoveInput ? runSpeed * _input.MoveInput.magnitude : 0, acceleration * Time.deltaTime);
        if (_curSpeed < 0.1f) _curSpeed = 0;
        
        _motion = Quaternion.AngleAxis(_targetYaw, transform.up) * Vector3.forward * (_curSpeed * Time.deltaTime);

        _motion.y = _motionY * Time.deltaTime;

        if (_detection.IsLedgeStuck)
            _motion += AvoidLedgeStuck() * Time.deltaTime;
        
        _controller.Move(_motion);
    }
    
    
    [NonSerialized] private float _rotationRef;
    public float RotationRef => _rotationRef;
    private void Rotation()
    {
        if (_input.IsMoveInput)
        {
            _targetYaw = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.Yaw;
            _targetYaw = Functions.ClampAngle(_targetYaw, -360, 360);
            _smoothYaw = Mathf.SmoothDampAngle(_smoothYaw,  _targetYaw, ref _rotationRef, angularTime);
            _forward = Quaternion.Euler(0, _smoothYaw, 0) * Vector3.forward;
        }
        
        transform.forward = _forward;
    }

    private void JumpAndFall()
    {
        if (_detection.IsGrounded && _motionY < 0)
            _motionY = -2;
        else
            _motionY -= gravity * Time.deltaTime;
        IsJump = false;
        
        if (_input.JumpInput && _detection.IsGrounded)
        {
            _motionY = Mathf.Sqrt(2 * jumpHeight * gravity);
            IsJump = true;
        }
    }

    private Vector3 AvoidLedgeStuck()
    {

        var avoidDirection = Vector3.zero;

        foreach (var hit in _detection.Hits)
        {
            if (!hit.collider) continue;

            avoidDirection += hit.normal;
        }

        return avoidDirection * ledgeStuckAvoidForce;
    }
}
