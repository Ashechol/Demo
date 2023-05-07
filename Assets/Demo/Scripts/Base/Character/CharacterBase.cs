using System;
using UnityEngine;
using Demo.Base;
using Demo.Utils;

namespace Demo.Base.Character
{
    public class CharacterBase : MonoBehaviour
    {
        internal Animator anim;
        internal AnimHandler animHandler;
        protected CharacterStateMachine stateMachine;

        protected virtual void Awake()
        {
            this.GetComponentSafe<AnimHandler>();
            anim = animHandler.anim;
        }
    }
}
