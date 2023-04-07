using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Functions
{
    private static Vector2 _vec2;
    public static Vector2 TempVec2(float x, float y, float t = 1)
    {
        _vec2.Set(x, y);
        return _vec2 * t;
    }

    public static T GetComponentSafe<T>(GameObject go) where T: Component
    {
        var component = go.GetComponent<T>();
        if (!component)
        {
            DebugLogType.Tips($"Missing {typeof(T).Name} creating it", Color.white);
            component = go.AddComponent<T>();
        }
        return component;
    }
}
