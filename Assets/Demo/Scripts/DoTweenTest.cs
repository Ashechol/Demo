using Demo.Utils;
using DG.Tweening;
using UnityEngine;
using Utils;

public class DoTweenTest : MonoBehaviour
{
    public bool isEditMode = false;
    public Vector2 end;
    public Vector2 control;
    public int resolution = 5;
    public float duration = 1;
    public AnimationCurve curve;

    [HideInInspector]
    public Vector3 start;
    
    /// <summary>
    /// World space control point
    /// </summary>
    public Vector3 Control
    {
        // get => transform.rotation * new Vector3(0, control.y, control.x) + transform.position;
        get => transform.TransformDirection(0, control.y, control.x) + transform.position;
        set
        {
            // value = Quaternion.Inverse(transform.rotation) * (value - transform.position);
            value = transform.InverseTransformDirection(value - transform.position);
            control.x = value.z; control.y = value.y;
        }
    }
    
    /// <summary>
    /// World space control point
    /// </summary>
    public Vector3 End
    {
        // get => transform.rotation * new Vector3(0, end.y, end.x) + transform.position;
        get => transform.TransformDirection(0, end.y, end.x) + transform.position;
        set
        {
            value = transform.InverseTransformDirection(value - transform.position);
            end.x = value.z; end.y = value.y;
        }
    }

    public Vector3[] points;

    private void Start()
    {
        transform
            .DOPath(points, duration, PathType.CatmullRom)
            .SetEase(curve)
            .SetLookAt(0);
    }

    public void EditCurve()
    {
        start = transform.position;
        points = Functions.BezierCurveBilinear(start, End, Control, resolution);
    }

    private void OnDrawGizmos()
    {
        if (isEditMode)
            DrawGizmos.DrawCurve(points);
        else
        {
            var localControl = new Vector3(0, control.y, control.x);
            var localEnd = new Vector3(0, end.y, end.x);
            var localPoints = Functions.BezierCurveBilinearLocal(localEnd, localControl, resolution);
            DrawGizmos.DrawCurve(transform, localPoints);
        }
    }
}
