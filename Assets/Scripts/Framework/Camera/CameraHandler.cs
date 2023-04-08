using System;
using Cinemachine;
using UnityEngine;

namespace Framework.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        private CinemachineStateDrivenCamera _stateCamera;
        
        [Header("Basic Settings")] 
        public Transform cameraRoot;
        
        private void Awake()
        {
            if (!cameraRoot) 
                cameraRoot = Functions.GetChildTransformSafe(transform, "CameraRoot", true);
            _stateCamera = ResourcesManager.Instance.LoadPrefab("MotionCamera").GetComponent<CinemachineStateDrivenCamera>();
        }
    }
}
