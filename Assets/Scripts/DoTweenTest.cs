using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;
using Utils;

public class DoTweenTest : MonoBehaviour
{
    public bool isEditMode = false;
    public Vector2 end = new Vector2(1, 0);
    public Vector2 control = new Vector2(0.5f, 1);
    public int resolution = 5;
    
    [HideInInspector]
    public Vector3 start;
    public Vector3 Control
    {
        get => new Vector3(0, control.y, control.x);
        set { control.x = value.z; control.y = value.y; }
    }
    public Vector3 End
    {
        get => new Vector3(0, end.y, end.x);
        set { end.x = value.z; end.y = value.y; }
    }

    private Vector3[] _points;

    private void Start()
    {
        
    }

    public void EditCurve()
    {
        start = transform.position;
        _points = Functions.BezierCurveBilinear(Vector3.zero, End, Control, resolution);
    }

    private void OnDrawGizmos()
    {
        start = transform.position;
        DrawGizmos.DrawCurve(start, _points, transform.rotation);
    }
}
