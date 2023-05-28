using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Demo.Framework.Animation
{
    [CreateAssetMenu(menuName = "Data/Animation/Animation Holder", fileName = "new animation holder")]
    public class AnimHolder : ScriptableObject
    {
        public ClipTransition[] idles;
        public MixerTransition2D move;
        public ClipTransition runToStand;
        public ClipTransition[] dashToStand;
        public ClipTransition[] jump;
        public LinearMixerTransition airBorne;
        public LinearMixerTransition landing;
    }
}
