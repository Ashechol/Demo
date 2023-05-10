using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public abstract class CharacterController : MonoBehaviour
    {
        protected Character _character;

        public CharacterController Possess(Character character)
        {
            _character = character;
            return this;
        }
    }
}
