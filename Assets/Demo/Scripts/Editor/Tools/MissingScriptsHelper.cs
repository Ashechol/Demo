using System.Collections.Generic;
using Demo.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class MissingScriptsHelper : EditorWindow
{
    public GameObject rootGo;
    private ReorderableList _missingScripts;
    private readonly List<GameObject> _gameObjects = new List<GameObject>();
    private Vector2 _scrollPosition;

    [MenuItem("Demo/Tools/Missing Scripts Helper")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(MissingScriptsHelper));
        window.Show();
    }

    private void OnEnable()
    {
        _missingScripts = new ReorderableList(_gameObjects, typeof(GameObject), true, 
            true, false, true);
        _missingScripts.multiSelect = true;
        _missingScripts.onSelectCallback += OnSelectCallback;
        _missingScripts.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Game Objects With Missing Scripts");
        };
    }

    private void OnGUI()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        
        GUILayout.Label("Select a root game object to remove all the missing scripts on its children");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        EditorGUILayout.LabelField("Root Object");
        rootGo = (GameObject)EditorGUILayout.ObjectField(rootGo, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Find Missing Scripts"))
        {
            _gameObjects.Clear();
            FindMissingScripts();
        }

        _missingScripts.DoLayoutList();
        
        if (GUILayout.Button("Remove"))
        {
            int cnt = 0;
            foreach (var go in _gameObjects)
                cnt += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            DebugLog.Editor($"Removed {cnt} missing scripts!", Color.white);
            _gameObjects.Clear();
        }
        
        GUILayout.EndScrollView();
    }

    private void FindMissingScripts()
    {
        if (!rootGo) return;
        var goTrans = rootGo.transform.GetComponentsInChildren<Transform>();
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
