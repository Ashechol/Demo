using Demo.Base;
using Demo.Utils.Debug;
using UnityEngine;

namespace Demo.Characters
{
    public class PlayerAnim : AnimHandler
    {
        # region Animator Parameter IDs
    
        private int _animLean;
        private int _animFallSpeed;

        #endregion
    
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

        private void OnEnable()
        {
            GUIStats.Instance.OnGUIStatsInfo.AddListener(OnAnimGUIInfo);
        }

        private void OnDisable()
        {
            GUIStats.Instance.OnGUIStatsInfo.RemoveListener(OnAnimGUIInfo);
        }

        public override void UpdateAnimParams()
        {
            if (!hasAnimator) return;

            // 倾斜度计算，利用 Player SmoothDamp 的相对速度除以一定值可以很轻松的求到倾斜度
            _leanAmount = Mathf.Clamp(_player.RotationRef / _leanNormalizeAmount, -1, 1);
            anim.SetFloat(_animLean, _leanAmount);
            anim.SetFloat(_animFallSpeed, _player.FallSpeed);
        }

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
}
