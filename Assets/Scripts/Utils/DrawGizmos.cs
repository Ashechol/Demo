using UnityEngine;

namespace Utils
{
    public struct DrawGizmos
    {
        public static void DrawCurve(Vector3[] points, Color? color = null)
        {
            Gizmos.color = color ?? Color.green;
            
            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                    Gizmos.DrawLine(points[i], points[i+1]);
        }

        public static void DrawCurve(Transform origin, Vector3[] points, Color? color = null)
        {
            Gizmos.color = color ?? Color.green;

            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                    Gizmos.DrawLine(origin.TransformDirection(points[i]), 
                                    origin.TransformDirection(points[i+1]));
        }
    }
}
