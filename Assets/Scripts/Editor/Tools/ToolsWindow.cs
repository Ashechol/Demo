using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class ToolsWindow : EditorWindow
{
    public GameObject goWithInvalidScripts;
    [MenuItem("Demo/Tools/Invalid Scripts Helper")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(ToolsWindow));
        window.titleContent = new GUIContent("Invalid Scripts Helper");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Game Object");
        goWithInvalidScripts = (GameObject)EditorGUILayout.ObjectField(goWithInvalidScripts, typeof(GameObject), true);
        GUILayout.EndHorizontal();
    }
}
