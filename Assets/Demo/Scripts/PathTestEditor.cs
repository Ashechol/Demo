using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathTest), true)]
public class PathTestEditor : Editor
{
    private PathTest _pathTest;
    private void Awake()
    {
        _pathTest = target as PathTest;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        for (int i = 0; i < _pathTest.path.Length; ++i)
        {
            _pathTest.path[i] = Handles.PositionHandle(_pathTest.path[i], Quaternion.identity);
        }
    }
}
