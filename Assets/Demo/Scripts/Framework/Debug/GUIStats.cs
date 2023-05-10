using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Demo.Framework.Debug
{
    /// GUIStats handles all the OnGUI Info stats
    public class GUIStats : MonoBehaviour
    {
        private static GUIStats _instance;
        private static bool _isApplicationQuit;

        public static GUIStats Instance
        {
            get
            {
                if (_isApplicationQuit)
                    return _instance;
                
                if (!_instance)
                {
                    _instance = FindObjectOfType<GUIStats>();
                    if (!_instance)
                    {
                        var go = new GameObject("[" + nameof(GUIStats) + "]");
                        _instance = go.AddComponent<GUIStats>();
                        DontDestroyOnLoad(go); 
                    }
                }

                return _instance;
            }
        }

        private bool _isShow;

        public UnityEvent OnGUIStatsInfo = new();

        private void Update()
        {
            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
                _isShow = !_isShow;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _isApplicationQuit = false;
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        private void OnGUI()
        {
            if (!_isShow) return;

            OnGUIStatsInfo?.Invoke();
        }
    }
}
