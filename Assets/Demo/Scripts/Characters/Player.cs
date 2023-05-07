using System;
using System.Collections;
using Demo.Base;
using Demo.Characters;
using Demo.Framework.Camera;
using Demo.Framework.Input;
using Demo.Utils;
using Demo.Utils.Debug;
using UnityEngine;

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
    public float dashSpeed = 8;
    public float acceleration = 15;
    public float angularTime = 0.5f;
    public float ledgeStuckAvoidForce = 0.5f;

    [Header("Jump")] 
    public float gravity = 20;
    public float jumpHeight = 1.8f;
    public float landingRecoveryHeight = 5;
    [Range(0, 1)] public float speedDecreaseRate = 0.5f;
    public AnimationCurve recoveryCurve;
    public float recoverySmoothTime = 1;

    private bool _isJump;
    private bool _canJump = true;
    private float _landingRecoverySpeed;

    /// Local horizontal direction
    private Vector3 _forward;

    private float _curSpeed;
    private float _targetYaw;

    private float _smoothYaw;

    // Local velocity
    private float _desiredSpeed;
    private Vector3 _motion;
    private float _motionY;
    private Vector3 _velocity;
    private float _prevSpeedY;
    private float _fallSpeed;
    private Vector3 _rotation;
    private float _curSpeedDecreaseRate = 1;

    [NonSerialized] private float _stepOffset;

    public float CurSpeed => _velocity.MagnitudeXZ();
    public bool IsDash { get; private set; }

    private bool _isStop;
    public bool IsStop
    {
        get
        {
            var result = _isStop;
            _isStop = false;
            return result;
        }
    }

    public bool IsJump
    {
        get
        {
            var result = _isJump;
            _isJump = false;
            return result;
        }
    }

    public float VelocityY => _controller.velocity.y;
    public bool IsGrounded => _detection.IsGrounded;
    public float FallSpeed => -_fallSpeed;

    #endregion

    private void Awake()
    {
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
        _anim = this.GetComponentSafe<PlayerAnim>();
        _detection = GetComponentInChildren<Detection>();

#if UNITY_EDITOR
        if (!_detection)
            DebugLog.LabelLog(_debugLabel, "Missing Detection Component In Children!", Verbose.Error);
#endif
    }

    private void OnEnable()
    {
        GUIStats.Instance.OnGUIStatsInfo.AddListener(OnPlayerGUIInfo);
    }

    private void OnDisable()
    {
        GUIStats.Instance.OnGUIStatsInfo.RemoveListener(OnPlayerGUIInfo);
    }

    private void Start()
    {
        var trans = transform;
        _forward = trans.forward;
        _prevSpeedY = VelocityY;
        _desiredSpeed = runSpeed;
        _stepOffset = _controller.stepOffset;
        _landingRecoverySpeed = Mathf.Sqrt(2 * landingRecoveryHeight * gravity);

        _input.OnMoveCanceled += StopMove;
        _input.OnDash += (isDash) => { _desiredSpeed = isDash ? dashSpeed : runSpeed; };
    }

    private void StopMove() => _isStop = true;

    private void Update()
    {
        Locomotion();
        _anim.UpdateAnimParams();
    }

    private void Locomotion()
    {
        Rotation();

        JumpAndFall();

        var moveInput = Functions.NearlyEqual(_desiredSpeed, runSpeed, 0.01f) ? _input.MoveInput.magnitude : 1;
        var targetSpeed = _input.IsMoveInput ? _desiredSpeed * moveInput : 0;
        _curSpeed = Mathf.Lerp(_curSpeed, targetSpeed, acceleration * Time.deltaTime);
        if (_curSpeed < 0.1f) _curSpeed = 0;

        _motion = Quaternion.AngleAxis(_targetYaw, transform.up) * Vector3.forward * (_curSpeed * _curSpeedDecreaseRate * Time.deltaTime);

        _motion.y = _motionY * Time.deltaTime;

        if (_detection.IsLedgeStuck)
            _motion += AvoidLedgeStuck() * Time.deltaTime;
        
        _controller.Move(_motion);
        _velocity = _controller.velocity;
    }

    [NonSerialized] private float _rotationRef;
    public float RotationRef => _rotationRef;

    private void Rotation()
    {
        if (_input.IsMoveInput && _curSpeed > 0.1f)
        {
            _targetYaw = Mathf.Atan2(_input.MoveInputX, _input.MoveInputY) * Mathf.Rad2Deg + _camera.Yaw;
            _targetYaw = Functions.ClampAngle(_targetYaw, -360, 360);
            _smoothYaw = Mathf.SmoothDampAngle(_smoothYaw, _targetYaw, ref _rotationRef, angularTime);
            _forward = Quaternion.Euler(0, _smoothYaw, 0) * Vector3.forward;
        }

        transform.forward = _forward;
    }

    private float _speedDecreaseRateRef;

    private void JumpAndFall()
    {
        // 重力模拟
        if (_detection.IsGrounded && _motionY < 0)
        {
            _motionY = -2;
            _controller.stepOffset = _stepOffset;
        }
        else
            _motionY -= gravity * Time.deltaTime;

        // 跳跃
        if (_input.JumpInput && _detection.IsGrounded && _canJump)
        {
            _motionY = Mathf.Sqrt(2 * jumpHeight * gravity);
            _isJump = true;
            _fallSpeed = 0.0f;
            _controller.stepOffset = 0.0f;
        }

        // 下落速度记录
        if (!_detection.IsGrounded && VelocityY < 0.3f)
        {
            _fallSpeed = _prevSpeedY;
            _prevSpeedY = VelocityY;
        }

        // 下落着陆移动减速
        LandingRecovery();
    }

    private void LandingRecovery()
    {
        var ratio = Mathf.Clamp(-_fallSpeed / _landingRecoverySpeed, 0, 1);
        
        if (_detection.IsHitGround)
            _curSpeedDecreaseRate = recoveryCurve.Evaluate(ratio);
        else if (Functions.InRange(_curSpeedDecreaseRate, 0.1f, 1))
        {
            _curSpeedDecreaseRate =
                Mathf.SmoothDamp(_curSpeedDecreaseRate, 1, ref _speedDecreaseRateRef, recoverySmoothTime);
        }
        else
            StartCoroutine(HeavyLanding(0.6f));
    }

    private IEnumerator HeavyLanding(float recoveringTime)
    {
        _canJump = false;
        _curSpeedDecreaseRate = 0;
        yield return new WaitForSeconds(recoveringTime);
        _curSpeedDecreaseRate = 1;
        _canJump = true;
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

    private void OnPlayerGUIInfo()
    {
        var style = new GUIStyle
        {
            fontSize = 30
        };
        GUILayout.Label($"<color=yellow>Speed decrease rate: {_curSpeedDecreaseRate}</color>", style);
        GUILayout.Label($"<color=yellow>Desired Speed: {_desiredSpeed}</color>", style);
        GUILayout.Label($"<color=yellow>Current Speed: {CurSpeed}</color>", style);
    }
}
