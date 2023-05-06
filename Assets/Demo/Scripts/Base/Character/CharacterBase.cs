using System;
using UnityEngine;
using Demo.Base;
using Demo.Utils;

namespace Demo.Base.Character
{
    public class CharacterBase : MonoBehaviour
    {
        internal Animator anim;
        internal AnimationHandler animHandler;

        private void Awake()
        {
            this.GetComponentSafe<AnimationHandler>();
            anim = animHandler.anim;
        }
    }
}
