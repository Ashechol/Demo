using System;
using System.Collections.Generic;
using Demo.Framework.Gameplay;
using UnityEngine;

namespace Demo.Framework.Attribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class EnumExtensionAttribute : System.Attribute
    {
        public EnumExtensionAttribute()
        {
            
        }
    }
}
