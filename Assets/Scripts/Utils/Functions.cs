using System;
using UnityEngine;
using Utils.Log;

namespace Utils
{
    public struct Functions
    {
        /// <summary>
        /// Get component, if it is null then attach it to gameObject
        /// </summary>
        /// <param name="go">component attach to</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("This method is obsolete. Use ExtendMethods.GetComponentSafe instead.", true)]
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

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle > 360) angle -= 360;
            if (angle < -360) angle += 360;

            return Mathf.Clamp(angle, min, max);
        }
    
        /// <summary>
        /// Get a bilinear Bezier point
        /// </summary>
        /// <param name="start">Start Point</param>
        /// <param name="end">End Point</param>
        /// <param name="control">Control Point</param>
        /// <param name="t">Time which between 0~1</param>
        /// <returns></returns>
        public static Vector3 BezierPointBilinear(Vector3 start, Vector3 end, Vector3 control, float t)
        {
            return (1 - t) * (1 - t) * start + 2 * (1 - t) * t * control + t * t * end;
        }
    
        /// <summary>
        /// Get bilinear Bezier curve local points' positions
        /// </summary>
        /// <param name="end">Local end point</param>
        /// <param name="control">Local control point</param>
        /// <param name="resolution">Curve resolution</param>
        /// <returns></returns>
        public static Vector3[] BezierCurveBilinearLocal(Vector3 end, Vector3 control, int resolution)
        {
            var points = new Vector3[resolution];
        
            for (var i = 0; i < resolution; ++i)
            {
                var t = (float)(i + 1) / resolution;
                points[i] = BezierPointBilinear(Vector3.zero, end, control, t);
            }
        
            return points;
        }
        
        /// <summary>
        /// Get bilinear Bezier curve world space points' positions
        /// </summary>
        /// <param name="start">Start point</param>
        /// <param name="end">End point</param>
        /// <param name="control">Control point</param>
        /// <param name="resolution">Curve resolution</param>
        /// <returns>World space points' positions</returns>
        public static Vector3[] BezierCurveBilinear(Vector3 start, Vector3 end, Vector3 control, int resolution)
        {
            var points = new Vector3[resolution];
            for (var i = 0; i < resolution; ++i)
            {
                var t = (float)(i + 1) / resolution;
                points[i] = BezierPointBilinear(start, end, control, t);
            }

            return points;
        }
    }
}
