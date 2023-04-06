using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class MissingScriptsHelper : EditorWindow
{
    public GameObject goWithMissingScripts;
    private ReorderableList _missingScripts;
    private readonly List<GameObject> _gameObjects = new List<GameObject>();
    
    [MenuItem("Demo/Tools/Missing Scripts Helper")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(MissingScriptsHelper));
        window.Show();
    }

    private void OnEnable()
    {
        _missingScripts = new ReorderableList(_gameObjects, typeof(GameObject), true, 
            true, true, true);
        _missingScripts.multiSelect = true;
        _missingScripts.onSelectCallback += OnSelectCallback;
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a game object to remove all the missing scripts on it");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Game Object");
        goWithMissingScripts = (GameObject)EditorGUILayout.ObjectField(goWithMissingScripts, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Find Missing Scripts"))
        {
            _gameObjects.Clear();
            FindMissingScripts();
        }
        
        _missingScripts.DoLayoutList();
        
        if (GUILayout.Button("Remove"))
        {
            
        }
    }

    void FindMissingScripts()
    {
        if (!goWithMissingScripts) return;
        var goTrans = goWithMissingScripts.transform.GetComponentsInChildren<Transform>();
        foreach (var trans in goTrans)
        {
            var components = trans.GetComponents<Component>();
            foreach (var component in components)
            {
                if (!component)
                {
                    _gameObjects.Add(trans.gameObject);
                    break;
                }
            }
        }
    }

    private void OnSelectCallback(ReorderableList list)
    {
        var t = list.selectedIndices;
        Selection.SetActiveObjectWithContext(_gameObjects[t[0]], null);
    }


}
