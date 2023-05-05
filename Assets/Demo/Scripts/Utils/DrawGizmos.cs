using UnityEngine;

namespace Demo.Utils
{
    public struct DrawGizmos
    {
        /// <summary>
        /// Draw a gizmos curve by using points in world space.
        /// </summary>
        public static void DrawCurve(Vector3[] points, Color? color = null)
        {
            Gizmos.color = color ?? Color.green;
            
            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                    Gizmos.DrawLine(points[i], points[i+1]);
        }
        
        /// <summary>
        /// Draw a gizmos curve by using points in local space.
        /// </summary>
        public static void DrawCurve(Transform origin, Vector3[] points, Color? color = null)
        {
            Gizmos.color = color ?? Color.green;

            var pos = origin.position;
            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                    Gizmos.DrawLine(origin.TransformDirection(points[i]) + pos, 
                                    origin.TransformDirection(points[i+1]) + pos);
        }
    }
}
