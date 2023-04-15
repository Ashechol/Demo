using UnityEngine;
using Utils.Log;

namespace Utils
{
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

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle > max) angle -= max;
            if (angle < min) angle += max;

            return angle;
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
        /// Get bilinear Bezier curve points
        /// </summary>
        /// <param name="start">Start Point</param>
        /// <param name="end">End Point</param>
        /// <param name="control">Control Point</param>
        /// <param name="resolution">Curve resolution</param>
        /// <returns></returns>
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
