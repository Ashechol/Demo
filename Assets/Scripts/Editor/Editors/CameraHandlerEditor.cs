using Framework.Camera;
using Unity.VisualScripting;
using UnityEditor;

[CustomEditor(typeof(CameraHandler))]
public class CameraHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (!(target as CameraHandler)?.cameraRoot)
        {
            EditorGUILayout.HelpBox("Need to set a CameraRoot!", MessageType.Warning);
        }
        base.OnInspectorGUI();
    }
}
