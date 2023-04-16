using UnityEditor;
using UnityEngine;
using Utils.Log;

[CustomEditor(typeof(DoTweenTest))]
public class DoTweenTestEditor : Editor
{
    private DoTweenTest _doTest;
    private SerializedProperty _isEditMode;
    private SerializedProperty _points;

    private void OnEnable()
    {
        _doTest = target as DoTweenTest;
        _isEditMode = serializedObject.FindProperty(nameof(_doTest.isEditMode));
        _points = serializedObject.FindProperty(nameof(_doTest.points));
    }

    public void OnSceneGUI()
    {
        if (!_doTest.isEditMode) return;
        
        _doTest.EditCurve();

        var transform = _doTest.transform;
        var position = transform.position;

        EditorGUI.BeginChangeCheck();
        
        Handles.color = Color.cyan;
        var newControl = Handles.Slider2D(_doTest.Control, 
            transform.right, transform.up, transform.forward, 
            0.05f, Handles.DotHandleCap, Vector2.one * 0.1f);
        
        var newEnd = Handles.Slider2D(_doTest.End, 
            transform.right, transform.up, transform.forward, 
            0.05f, Handles.DotHandleCap, Vector2.one * 0.1f);
        
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_doTest, "Change Bezier curve");
            _doTest.Control = newControl;
            _doTest.End = newEnd;
        }
        
        Handles.DrawLine(position, _doTest.Control);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_isEditMode);

        if (_doTest.isEditMode)
        {
            _doTest.end = EditorGUILayout.Vector2Field("End Point", _doTest.end);
            _doTest.control = EditorGUILayout.Vector2Field("Control Point", _doTest.control);
            _doTest.resolution = EditorGUILayout.IntField("Resolution", _doTest.resolution);
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_points);
            EditorGUI.EndDisabledGroup();
            
            // 没有使用 EditorGUILayout.PropertyField 的属性拖动改值的情况
            // 不会刷新场景的 GUI 组件，所以需要 RepaintAll
            SceneView.RepaintAll();
        }

        _doTest.duration = EditorGUILayout.FloatField("Duration", _doTest.duration);
        _doTest.curve = EditorGUILayout.CurveField("Ease Curve", _doTest.curve);
        
        serializedObject.ApplyModifiedProperties();
    }
}
