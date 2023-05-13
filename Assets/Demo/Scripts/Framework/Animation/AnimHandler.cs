using Demo.Framework.Utils;
using Demo.Framework.Debug;
using UnityEngine;

namespace Demo.Framework.Animation
{
    public abstract class AnimHandler : MonoBehaviour
    {
        protected Animator anim;
        protected bool hasAnimator;

        protected static DebugLabel debugLabel = new("AnimationHandler", Color.cyan); 

        protected virtual void Awake()
        {
            anim = transform.GetComponentInChildren<Animator>();
            hasAnimator = anim;
        
            if (!hasAnimator) 
                DebugLog.LabelLog(debugLabel, $"Missing animator in {gameObject.name}!", Verbose.Warning);
        }

        protected virtual void Start()
        {
            RegisterAnimID();

        }

        protected abstract void RegisterAnimID();

        public abstract void UpdateAnimParams();
    }
}