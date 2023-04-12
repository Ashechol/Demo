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
        public float mouseYSpeed = 30;
        public float mouseXSpeed = 30;
        public float pitchMax = 60;
        public float pitchMin = -30;

        private void Awake()
        {
            // Initialize();
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            
            _input = Functions.GetComponentSafe<InputHandler>(gameObject);
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            
            _yaw = cameraRoot.rotation.eulerAngles.y;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            
            CameraRotation();
        }
        
        private void OnValidate()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            if (!cameraRoot)
                cameraRoot = Functions.GetChildTransformSafe(transform, "CameraRoot", true);
            if (!stateCamera)
                stateCamera = ResourceLoader.GetPrefabComponent<StateCamera>("MotionCamera", "MotionCamera");
            
            cameraRoot.localPosition = Vector3.zero;
            stateCamera.Follow = cameraRoot;
            stateCamera.transform.SetParent(transform);
        }

        private void CameraRotation()
        {
            _yaw += _input.YawInput * Time.deltaTime * mouseXSpeed;
            _yaw = Functions.ClampAngle(_yaw, 0, 360);
            _pitch += _input.PitchInput * Time.deltaTime * mouseYSpeed;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
            
            // 必须是 global rotation 才能解绑子物体的旋转和父物体
            cameraRoot.rotation = Quaternion.Euler(_pitch, _yaw, 0);
        }
    }
}
