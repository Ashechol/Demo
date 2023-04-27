using System;
using Demo.Base;
using Demo.Utils;
using DG.Tweening;
using TreeEditor;
using UnityEngine;

public class PlayerAnim : AnimationHandler
{
    private int _animSpeedXZ;
    private int _animLean;
    private int _animJump;
    private int _animSpeedY;
    private int _animGrounded;
    private int _animFallHeight;
    private Player _player;
    
    private float _leanAmount;
    private float _fallHeight;

    [SerializeField] private float _leanNormalizeAmount = 150;

    protected override void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent(out _player))
            DebugLog.LabelLog(debugLabel, $"Missing Player in {gameObject.name}!", Verbose.Error);
    }

    protected override void RegisterAnimID()
    {
        _animSpeedXZ = Animator.StringToHash("speedXZ");
        _animLean = Animator.StringToHash("lean");
        _animJump = Animator.StringToHash("jump");
        _animSpeedY = Animator.StringToHash("speedY");
        _animGrounded = Animator.StringToHash("grounded");
        _animFallHeight = Animator.StringToHash("fallHeight");
        
    }

    public override void UpdateAnimParams()
    {
        if (!hasAnimator) return;
        
        GetFallHeight();

        anim.SetFloat(_animSpeedXZ, _player.CurSpeed);
        
        // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值
        // 可以很轻松的求到倾斜度
        _leanAmount = Mathf.Clamp(_player.RotationRef / _leanNormalizeAmount, -1, 1);
        anim.SetFloat(_animLean, _leanAmount);
        
        anim.SetBool(_animJump, _player.IsJump);
        anim.SetFloat(_animSpeedY, _player.VelocityY);
        anim.SetBool(_animGrounded, _player.IsGrounded);
        anim.SetFloat(_animFallHeight, _fallHeight);
    }

    private RaycastHit _prevHit;
    private void GetFallHeight()
    {
        if (!_player.IsGrounded)
        {
            Physics.Raycast(_player.transform.position, Vector3.down, out var hit);
            _fallHeight = Mathf.Max(_fallHeight, hit.distance);

            if (_prevHit.colliderInstanceID != hit.colliderInstanceID)
                _fallHeight = 0;

            _prevHit = hit;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Ground Locomotion") && _player.IsGrounded)
            _fallHeight = 0;
        
        
    }

    private void OnGUI()
    {
        var style = new GUIStyle
        {
            fontSize = 30
        };
        GUILayout.Label($"<color=yellow>Rotation speed reference：{_player.RotationRef}</color>", style);
    }
}
