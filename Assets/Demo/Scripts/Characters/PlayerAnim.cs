using System;
using Demo.Utils;
using DG.Tweening;
using UnityEngine;

public class PlayerAnim : AnimationHandler
{
    private int _speedXZ;
    private int _lean;
    private int _jump;
    private int _speedY;
    private int _grounded;
    private Player _player;

    private float _lastYaw;
    private float _leanAmount;

    protected override void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent(out _player))
            DebugLog.LabelLog(debugLabel, $"Missing Player in {gameObject.name}!", Verbose.Error);
    }

    protected override void Start()
    {
        base.Start();
        _lastYaw = _player.CurrentYaw;
    }

    protected override void RegisterAnimID()
    {
        _speedXZ = Animator.StringToHash("speedXZ");
        _lean = Animator.StringToHash("lean");
        _jump = Animator.StringToHash("jump");
        _speedY = Animator.StringToHash("speedY");
        _grounded = Animator.StringToHash("grounded");
    }

    public override void UpdateAnimParams()
    {
        if (!hasAnimator) return;

        anim.SetFloat(_speedXZ, _player.CurSpeed);
        
        // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值
        // 可以很轻松的求到倾斜度
        _leanAmount = Mathf.Clamp(_player.RotationRef / 150, -1, 1);
        anim.SetFloat(_lean, _leanAmount);
        
        anim.SetBool(_jump, _player.IsJump);
        anim.SetFloat(_speedY, _player.VelocityY);
        anim.SetBool(_grounded, _player.IsGrounded);
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
