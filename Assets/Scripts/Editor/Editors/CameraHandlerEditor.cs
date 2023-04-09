using Framework;
using Framework.Camera;
using UnityEditor;
using UnityEngine;
using Utils.Log;

using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

[CustomEditor(typeof(CameraHandler))]
public class CameraHandlerEditor : Editor
{
    private CameraHandler _camera;
    private bool _initialized;
    private DebugLabel _debugLabel = new("CameraHandlerEditor", Color.cyan, Color.white);

    private void OnEnable()
    {
        _camera = target as CameraHandler;
    }

    public override void OnInspectorGUI()
    {
        _initialized = _camera.cameraRoot && _camera.stateCamera;

        if (!_initialized && GUILayout.Button("Initialize")) _camera.Initialize();

        if (!_camera.cameraRoot)
            EditorGUILayout.HelpBox("Need to set a CameraRoot!", MessageType.Warning);
        if (!_camera.stateCamera)
            EditorGUILayout.HelpBox("Need to set a StateDrivenCamera!", MessageType.Warning);

        base.OnInspectorGUI();
    }
}
