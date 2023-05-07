using System.Collections.Generic;
using Demo.Utils.Debug;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Demo.Base
{
    public abstract class AnimHandler : MonoBehaviour
    {
        [HideInInspector] public Animator anim;
        public AnimatorController animController;
        protected bool hasAnimator;

        protected readonly Dictionary<string, int> animTriggerID = new();

        protected static DebugLabel debugLabel = new("AnimHandler", Color.cyan); 

        protected virtual void Awake()
        {
            anim = transform.GetComponentInChildren<Animator>();
            hasAnimator = anim;
        
            if (!hasAnimator) 
                DebugLog.LabelLog(debugLabel, $"Missing animator in {gameObject.name}!", Verbose.Warning);
        }

        protected virtual void Start()
        {
            anim.runtimeAnimatorController = animController;

            foreach (var param in animController.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                    animTriggerID[param.name] = param.nameHash;
            }
        }

        public void SetTrigger(string trigger)
        {
            if (animTriggerID.ContainsKey(trigger))
                anim.SetTrigger(animTriggerID[trigger]);
#if UNITY_EDITOR
            else
            {
                DebugLog.LabelLog(debugLabel, $"{animController.name} missing parameter {trigger}, adding it", Verbose.Warning);
                animController.AddParameter(trigger, AnimatorControllerParameterType.Trigger);
            }
#endif
        }

        public abstract void UpdateAnimParams();
    }
}
