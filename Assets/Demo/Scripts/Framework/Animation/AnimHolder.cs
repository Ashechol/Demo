using Animancer;
using UnityEngine;

namespace Demo.Framework.Animation
{
    [CreateAssetMenu(menuName = "Data/Animation/Animation Holder", fileName = "new animation holder")]
    public class AnimHolder : ScriptableObject
    {
        public ClipTransition[] idles;
        public MixerTransition2D move;
        public ClipTransition jumpStart;
        public LinearMixerTransition airBorne;
        public ClipTransitionSequence landing;
    }
}
