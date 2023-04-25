using Demo.Utils;
using DG.Tweening;
using UnityEngine;

public class PlayerAnim : AnimationHandler
{
    private int _speedXZ;
    private int _lean;
    private Player _player;

    private float _lastTurningAngle;
    private float _leanAmount = 0;

    protected override void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent(out _player))
            DebugLog.LabelLog(debugLabel, $"Missing Player in {gameObject.name}!", Verbose.Error);
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

        CalculateLeanAmount();
        anim.SetFloat(_lean, _leanAmount);
    }

    private void CalculateLeanAmount()
    {
        Debug.Log($"cur: {_player.CurrentTurningAngle}, tar: {_player.TurningAngle}");
        _leanAmount = Mathf.Lerp(_leanAmount, _player.CurrentTurningAngle - _player.TurningAngle, Time.deltaTime);
    }
}
