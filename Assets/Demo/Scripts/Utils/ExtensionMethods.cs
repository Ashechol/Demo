using UnityEngine;
using UnityEngine.Animations;
using Utils.Log;

namespace Utils
{
    public static class ExtensionMethods
    {
        #region Get Compoent

        /// <summary>
        /// Get component, if it is null then attach it to gameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetComponentSafe<T>(this GameObject go) where T: Component
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
        /// Get component, if it is null then attach it to gameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetComponentSafe<T>(this Transform transform) where T: Component
        {
            var component = transform.GetComponent<T>();
            if (!component)
            {
                DebugLog.Tips($"Attaching {typeof(T).Name} to {transform.name}.", Color.white);
                component = transform.gameObject.AddComponent<T>();
            }
            return component;
        }
    
        /// <summary>
        /// Get component, if it is null then attach it to gameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetComponentSafe<T>(this Component comp) where T: Component
        {
            var component = comp.GetComponent<T>();
            if (!component)
            {
                DebugLog.Tips($"Attaching {typeof(T).Name} to {comp.name}.", Color.white);
                component = comp.gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Get child's transform by name, create it if not find the child. 
        /// </summary>
        /// <param name="name">child's name</param>
        /// <param name="skipFind">skip finding child, just create a new one</param>
        public static Transform GetChildTransformSafe(this Transform parent, string name, bool skipFind = false)
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

        #endregion
        
        
    }
}
