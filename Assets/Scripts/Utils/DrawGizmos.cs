using UnityEngine;

namespace Utils
{
    public struct DrawGizmos
    {
        public static void DrawCurve(Vector3 origin, Vector3[] points, Color? color = null)
        {
            Gizmos.color = color ?? Color.green;
            
            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                    Gizmos.DrawLine(origin + points[i], origin + points[i+1]);
        }
        
        public static void DrawCurve(Vector3 origin, Vector3[] points, Quaternion rotation,Color? color = null)
        {
            Gizmos.color = color ?? Color.green;
            
            if (points.Length > 1)
                for (var i = 0; i < points.Length - 1; ++i)
                {
                    Gizmos.DrawLine(origin + rotation * points[i], origin + rotation * points[i+1]);
                }
        }
        
        
    }
}
