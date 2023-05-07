using System;
using System.Collections;
using System.Collections.Generic;
using Demo.Base;
using Demo.Base.Character;
using Demo.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterBase))]
public class CharacterBaseEditor : Editor
{
    private CharacterBase _character;
    
    private void OnEnable()
    {
        _character = target as CharacterBase;
        
        // Add Detection to character game object
        if (_character)
        {
            if (!_character.GetComponentInChildren<Detection>())
            {
                var detection = _character.transform.GetChildTransformSafe("Detection");
                detection.AddComponent<Detection>();
            }
        }
    }

    private void OnDisable()
    {
        if (target == null && !Application.genuine)
        {
            DestroyImmediate(_character.transform.GetComponentInChildren<Detection>().gameObject);
        }
    }
}
