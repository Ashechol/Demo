using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class Controller : MonoBehaviour
    {
        public Character character;

        protected virtual void Awake()
        {
            character = GetComponentInChildren<Character>();
        }
    }
}
