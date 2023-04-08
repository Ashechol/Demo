using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public struct Functions
{
    private static Vector2 _vec2;
    public static Vector2 TempVec2(float x, float y, float scale = 1)
    {
        _vec2.Set(x, y);
        return _vec2 * scale;
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

    /// <summary>
    /// Get child's transform by name, create it if not find the child. 
    /// </summary>
    /// <param name="parent">child's parent</param>
    /// <param name="name">child's name</param>
    /// <param name="skipFind">skip finding child, just create a new one</param>
    /// <returns></returns>
    public static Transform GetChildTransformSafe(Transform parent, string name, bool skipFind = false)
    {
        Transform res = null;
        if (!skipFind) res = parent.Find(name);

        if (!res)
        {
            DebugLogType.Tips($"Missing {name} adding it to {parent.name}", Color.white);
            res = new GameObject(name).transform;
            res.SetParent(parent);
        }
        
        return res;
    }
}
