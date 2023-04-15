using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoTweenTest))]
public class DoTweenTestEditor : Editor
{
    private DoTweenTest _doTest;
    private SerializedProperty _isEditMode;

    private void OnEnable()
    {
        _doTest = target as DoTweenTest;
        _isEditMode = serializedObject.FindProperty(nameof(_doTest.isEditMode));
    }

    public void OnSceneGUI()
    {
        if (!_doTest.isEditMode) return;
        
        _doTest.EditCurve();

        var transform = _doTest.transform;
        var position = transform.position;
        var rotation = transform.rotation;
        
        EditorGUI.BeginChangeCheck();
        
        Handles.color = Color.cyan;
        var newControl = Handles.Slider2D(position + rotation * _doTest.Control, 
            transform.right, transform.up, transform.forward, 
            0.05f, Handles.DotHandleCap, Vector2.one * 0.1f) - position;
        
        var newEnd = Handles.Slider2D(position + rotation * _doTest.End, 
            transform.right, transform.up, transform.forward, 
            0.05f, Handles.DotHandleCap, Vector2.one * 0.1f) - position;
        Handles.DrawLine(_doTest.start, position + rotation * _doTest.Control);
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_doTest, "Change Bezier curve");
            _doTest.Control = Quaternion.Inverse(rotation) * newControl;
            _doTest.End = Quaternion.Inverse(rotation) * newEnd;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_isEditMode);
        serializedObject.ApplyModifiedProperties();
        
        EditorGUI.BeginDisabledGroup(!_doTest.isEditMode);

        _doTest.end = EditorGUILayout.Vector2Field("End Point", _doTest.end);
        _doTest.control = EditorGUILayout.Vector2Field("control Point", _doTest.control);
        _doTest.resolution = EditorGUILayout.IntField("Resolution", _doTest.resolution);
        
        EditorGUI.EndDisabledGroup();

        
    }
}
