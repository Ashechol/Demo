using System;
using Cinemachine;
using Inputs;
using UnityEngine;

using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

namespace Framework.Camera
{
    [ExecuteInEditMode]
    public class CameraHandler : MonoBehaviour
    {
        private InputHandler _input;
        private float _yaw;
        private float _pitch;
        
        [Header("Basic Settings")]
        public CinemachineStateDrivenCamera stateCamera;
        public Transform cameraRoot;

        [Header("Control Settings")]
        public float pitchMax;
        public float pitchMin;

        private void Awake()
        {
            _input = Functions.GetComponentSafe<InputHandler>(gameObject);
            Initialize();
        }

        private void Start()
        {
            _yaw = cameraRoot.rotation.eulerAngles.y;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            
            CameraRotation();
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

        public void CameraRotation()
        {
            _yaw += _input.YawInput * Time.deltaTime * 50;
            _yaw = Functions.ClampAngle(_yaw, 0, 360);
            cameraRoot.rotation = Quaternion.Euler(0, _yaw, 0);
        }
    }
}
