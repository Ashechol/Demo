using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Functions
{
    public static T GetComponentSafe<T>(GameObject go) where T: Component
    {
        var component = go.GetComponent<T>();
        if (!component)
        {
            DebugLogType.Tips($"Missing {typeof(T).Name} creating it", Color.white, DebugLogType.Verbose.Warning);
            component = go.AddComponent<T>();
        }
        return component;
    }
}
