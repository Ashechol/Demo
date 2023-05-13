using System;
using Demo.Framework.Animation;
using Demo.Framework.Utils;
using Demo.Framework.Debug;
using UnityEngine;

public class PlayerAnim : AnimHandler
{
    # region Animator Parameter IDs
    
    private int _animSpeedXZ;
    private int _animLean;
    private int _animJump;
    private int _animSpeedY;
    private int _animGrounded;
    private int _animFallHeight;
    private int _animFallSpeed;
    private int _animStopRun;
    private int _animStopDash;
    private Player _player;
    
    #endregion
    
    private float _leanAmount;
    private float _fallHeight;

    [SerializeField] private float _leanNormalizeAmount = 150;

    protected override void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent(out _player))
            DebugLog.LabelLog(debugLabel, $"Missing Player in {gameObject.name}!", Verbose.Error);
    }

    private void OnEnable()
    {
        GUIStats.Instance.OnGUIStatsInfo.AddListener(OnAnimGUIInfo);
    }

    private void OnDisable()
    {
        GUIStats.Instance.OnGUIStatsInfo.RemoveListener(OnAnimGUIInfo);
    }

    protected override void RegisterAnimID()
    {
        _animSpeedXZ = Animator.StringToHash("speedXZ");
        _animLean = Animator.StringToHash("lean");
        _animJump = Animator.StringToHash("jump");
        _animSpeedY = Animator.StringToHash("speedY");
        _animGrounded = Animator.StringToHash("grounded");
        _animFallHeight = Animator.StringToHash("fallHeight");
        _animFallSpeed = Animator.StringToHash("fallSpeed");
        _animStopRun = Animator.StringToHash("stopRun");
        _animStopDash = Animator.StringToHash("stopDash");
    }

    public override void UpdateAnimParams()
    {
        if (!hasAnimator) return;
        
        // GetFallHeight();

        anim.SetFloat(_animSpeedXZ, _player.CurSpeed);
        
        // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值可以很轻松的求到倾斜度
        _leanAmount = Mathf.Clamp(_player.RotationRef / _leanNormalizeAmount, -1, 1);
        anim.SetFloat(_animLean, _leanAmount);

        if (_player.IsJump) anim.SetTrigger(_animJump);

        anim.SetFloat(_animSpeedY, _player.VelocityY);
        anim.SetBool(_animGrounded, _player.IsGrounded);
        anim.SetFloat(_animFallHeight, _fallHeight);
        anim.SetFloat(_animFallSpeed, _player.FallSpeed);
    }

    // private RaycastHit _prevHit;

    // private void GetFallHeight()
    // {
    //     if (!_player.IsGrounded)
    //     {
    //         Physics.Raycast(_player.transform.position, Vector3.down, out var hit);
    //         _fallHeight = Mathf.Max(_fallHeight, hit.distance);
    //
    //         if (_prevHit.colliderInstanceID != hit.colliderInstanceID)
    //             _fallHeight = 0;
    //
    //         _prevHit = hit;
    //     }
    //
    //     if (anim.GetCurrentAnimatorStateInfo(0).IsName("Ground Locomotion") && _player.IsGrounded)
    //         _fallHeight = 0;
    // }

    private void OnAnimGUIInfo()
    {
        var style = new GUIStyle
        {
            fontSize = 30
        };
        GUILayout.Label($"<color=yellow>Rotation speed reference：{_player.RotationRef}</color>", style);
        GUILayout.Label($"<color=yellow>Fall speed：{_player.FallSpeed}</color>", style);
    }
}
