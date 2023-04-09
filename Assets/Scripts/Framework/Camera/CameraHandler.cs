using System;
using Cinemachine;
using UnityEngine;

using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

namespace Framework.Camera
{
    [ExecuteInEditMode]
    public class CameraHandler : MonoBehaviour
    {
        [Header("Basic Settings")]
        public CinemachineStateDrivenCamera stateCamera;
        public Transform cameraRoot;

        private void Awake()
        {
            Initialize();
        }
        
        private void Start()
        {
            
        }

        public void Initialize()
        {
            if (!cameraRoot)
                cameraRoot = Functions.GetChildTransformSafe(transform, "CameraRoot", true);
            if (!stateCamera)
                stateCamera = ResourceManager.Instance.GetPrefabComponent<StateCamera>("MotionCamera", "MotionCamera");
            
            cameraRoot.localPosition = Vector3.zero;
            stateCamera.Follow = cameraRoot;
            stateCamera.transform.SetParent(transform);
            
            if (Application.isEditor && ResourceManager.HasInstance)
                DestroyImmediate(ResourceManager.Instance.gameObject);
        }
    }
}
