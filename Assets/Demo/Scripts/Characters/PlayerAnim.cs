using System;
using Demo.Utils;
using DG.Tweening;
using UnityEngine;

public class PlayerAnim : AnimationHandler
{
    private int _speedXZ;
    private int _lean;
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
    }

    public override void UpdateAnimParams()
    {
        if (!hasAnimator) return;

        anim.SetFloat(_speedXZ, _player.CurSpeed);
        
        // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值
        // 可以很轻松的求到倾斜度
        _leanAmount = Mathf.Clamp(_player.RotationRef / 150, -1, 1);
        anim.SetFloat(_lean, _leanAmount);
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
