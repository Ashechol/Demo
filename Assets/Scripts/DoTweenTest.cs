using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class DoTweenTest : MonoBehaviour
{
    public bool isEditMode = false;
    public Vector2 end = new Vector2(1, 0);
    public Vector2 control = new Vector2(0.5f, 1);
    public int resolution = 5;
    public float duration = 1;
    public AnimationCurve curve;

    [HideInInspector]
    public Vector3 start;
    
    /// <summary>
    /// world space control point
    /// </summary>
    public Vector3 Control
    {
        get => transform.rotation * new Vector3(0, control.y, control.x) + transform.position;
        set
        {
            value = Quaternion.Inverse(transform.rotation) * (value - transform.position);
            control.x = value.z; control.y = value.y;
        }
    }
    
    /// <summary>
    /// world space control point
    /// </summary>
    public Vector3 End
    {
        get => transform.rotation * new Vector3(0, end.y, end.x) + transform.position;
        set
        {
            value = Quaternion.Inverse(transform.rotation) * (value - transform.position);
            end.x = value.z; end.y = value.y;
        }
    }

    public Vector3[] points;

    private void Start()
    {
        transform
            .DOPath(points, duration, PathType.Linear)
            .SetEase(curve)
            .SetLookAt(0);
    }

    private void Update()
    {
        
    }

    public void EditCurve()
    {
        start = transform.position;
        points = Functions.BezierCurveBilinear(start, End, Control, resolution);
    }

    private void OnDrawGizmos()
    {
        DrawGizmos.DrawCurve(points);
    }
}
