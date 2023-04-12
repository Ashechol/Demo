using System;
using Cinemachine;
using Inputs;
using UnityEngine;
using Utils.Log;
using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

namespace Framework.Camera
{
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
            _input = Functions.GetComponentSafe<InputHandler>(gameObject);
            // Initialize();
        }

        private void Start()
        {
            _yaw = cameraRoot.rotation.eulerAngles.y;
        }

        private void Update()
        {
            CameraRotation();
        }

        public void Initialize()
        {
            if (!cameraRoot)
                cameraRoot = Functions.GetChildTransformSafe(transform, "CameraRoot");
            if (!stateCamera)
                stateCamera = ResourceLoader.GetPrefabComponent<StateCamera>("MotionCamera", transform, "MotionCamera");

            cameraRoot.localPosition = Vector3.zero;
            stateCamera.Follow = cameraRoot;
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
