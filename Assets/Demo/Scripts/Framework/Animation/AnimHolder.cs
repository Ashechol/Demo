using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Demo.Framework.Animation
{
    [CreateAssetMenu(menuName = "Data/Animation/Animation Holder", fileName = "new animation holder")]
    public class AnimHolder : ScriptableObject
    {
        [Header("Normal")]
        
        public ClipTransition[] idles;
        public MixerTransition2D move;
        public ClipTransition runToStand;
        public ClipTransition[] dashToStand;
        
        /// 0: jump; 1: second jump
        public ClipTransition[] jump;
        
        public LinearMixerTransition airBorne;
        public LinearMixerTransition landing;
        
        [Header("Combat")]
        
        /// 0 : stand draw; 1 : walk draw
        public ClipTransition[] drawWeapon = new ClipTransition[2];

        public ClipTransition[] idlesWeapon = new ClipTransition[3];
        public MixerTransition2D moveWeapon;
        public ClipTransition runToStandWeapon;

        public ClipTransition[] jumpWeapon;
        public LinearMixerTransition airBorneWeapon;
        public LinearMixerTransition landingWeapon;

        public ClipTransition[] comboAttacks;
        public ClipTransition[] chargeAttack;
        public ClipTransition[] skillAttackA;
        public ClipTransition[] skillAttackB;
    }
}
