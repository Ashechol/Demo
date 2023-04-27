using System;
using Demo.Utils;
using DG.Tweening;
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
    private readonly RaycastHit[] _hit = new RaycastHit[1];
    private float _fallHeight;
    private bool _landing;

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

        _landing = anim.GetCurrentAnimatorStateInfo(0).IsName("Land Recovery");

        anim.SetFloat(_animSpeedXZ, _player.CurSpeed);
        
        // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值
        // 可以很轻松的求到倾斜度
        _leanAmount = Mathf.Clamp(_player.RotationRef / 150, -1, 1);
        anim.SetFloat(_animLean, _leanAmount);
        
        anim.SetBool(_animJump, _player.IsJump);
        anim.SetFloat(_animSpeedY, _player.VelocityY);
        anim.SetBool(_animGrounded, _player.IsGrounded);
        anim.SetFloat(_animFallHeight, _fallHeight);
    }

    private void GetFallHeight()
    {
        if (_player.VelocityY < 0)
        {
            Physics.RaycastNonAlloc(_player.transform.position, Vector3.down, _hit, Mathf.Infinity);
            if (_hit.Length > 0)
                _fallHeight = Mathf.Max(_fallHeight, _hit[0].distance);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            _fallHeight = 0;
    }

    private void OnGUI()
    {
        var style = new GUIStyle
        {
            fontSize = 30
        };
        GUILayout.Label($"<color=yellow>Rotation speed reference：{_player.RotationRef}</color>\n", style);
    }
}
