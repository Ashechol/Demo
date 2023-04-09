using UnityEngine;
using Utils.Log;

public struct Functions
{
    private static Vector2 _vec2;
    public static Vector2 TempVec2(float x, float y, float scale = 1)
    {
        _vec2.Set(x, y);
        return _vec2 * scale;
    }
    
    /// <summary>
    /// Get component, if it is null then attach it to gameObject
    /// </summary>
    /// <param name="go">component attach to</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetComponentSafe<T>(GameObject go) where T: Component
    {
        var component = go.GetComponent<T>();
        if (!component)
        {
            DebugLog.Tips($"Attaching {typeof(T).Name} to {go.name}.", Color.white);
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
            DebugLog.Tips($"Attaching {name} to {parent.name}.", Color.white);
            res = new GameObject(name).transform;
            res.SetParent(parent);
        }
        
        return res;
    }
}
