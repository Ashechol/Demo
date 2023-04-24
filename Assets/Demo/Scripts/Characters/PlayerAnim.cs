using Demo.Utils;
using UnityEngine;

public class PlayerAnim : AnimationHandler
{
    private int _speedXZ;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent(out _player))
            DebugLog.LabelLog(debugLabel, $"Missing Player in {gameObject.name}!", Verbose.Error);
    }

    protected override void RegisterAnimID()
    {
        _speedXZ = Animator.StringToHash("speedXZ");
    }

    public override void UpdateAnimParams()
    {
        if (!hasAnimator) return;

        anim.SetFloat(_speedXZ, _player.CurSpeed);
    }
}
