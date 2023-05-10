using Demo.Framework.Debug;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

namespace Demo.Framework.Camera
{
    [CustomEditor(typeof(CameraHandler))]
    public class CameraHandlerEditor : Editor
    {
        private CameraHandler _camera;
        private bool _initialized;
        private DebugLabel _debugLabel = new("CameraHandlerEditor", Color.cyan, Color.white);

        private void OnEnable()
        {
            _camera = target as CameraHandler;
        
            // Initialize required game object
            if (_camera != null)
            {
                _initialized = _camera.cameraRoot && _camera.stateCamera;
                if (!_initialized)
                {
                    DebugLog.Editor("Initialize required game object for CameraHandler", Color.white, Verbose.Log);
                    _camera.Initialize();
                }
            }
        }

        private void OnDisable()
        {
            // OnDisable 会在每次 Inspector 组件内容更新的时候调用
            // 当 target 为 null 的时候 Component 才被真正的移除
            if (target == null && !_camera.IsDestroyed())
            {
                DestroyImmediate(_camera.cameraRoot.gameObject);
                DestroyImmediate(_camera.stateCamera.gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            if (!_camera.cameraRoot)
                EditorGUILayout.HelpBox("Need to set a CameraRoot!", MessageType.Warning);
            if (!_camera.stateCamera)
                EditorGUILayout.HelpBox("Need to set a StateDrivenCamera!", MessageType.Warning);

            base.OnInspectorGUI();
        }
    }
}
