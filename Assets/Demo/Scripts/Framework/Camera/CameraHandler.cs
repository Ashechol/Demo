using Cinemachine;
using Demo.Framework.Input;
using Demo.Framework.Utils;
using Framework;
using UnityEngine;
using UnityEngine.Serialization;
using StateCamera = Cinemachine.CinemachineStateDrivenCamera;

namespace Demo.Framework.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [HideInInspector] public float yaw;
        [HideInInspector] public float pitch;
        [HideInInspector] public float roll;

        public float Yaw => cameraRoot.eulerAngles.y;

        [Header("Basic Settings")]
        public CinemachineStateDrivenCamera stateCamera;
        public Transform cameraRoot;

        private void Start()
        {
            yaw = cameraRoot.rotation.eulerAngles.y;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void LateUpdate()
        {
            // 必须是 global rotation 才能解绑子物体的旋转和父物体
            cameraRoot.rotation = Quaternion.Euler(pitch, yaw, roll);  
        }

        public void Initialize()
        {
            if (!cameraRoot)
                cameraRoot = transform.GetChildTransformSafe("CameraRoot");
            if (!stateCamera)
                stateCamera = ResourceLoader.GetPrefabComponent<StateCamera>("MotionCamera", transform, "MotionCamera");

            cameraRoot.localPosition = Vector3.zero;
            stateCamera.Follow = cameraRoot;
        }

        // private void CameraRotation()
        // {
        //     // 处理鼠标输入不能乘以 Time.deltaTime
        //     // _yaw += _input.YawInput * mouseXSpeed;
        //     _yaw += _input.YawInputFixed * mouseXSpeed;
        //     _yaw = Functions.ClampAngle(_yaw, float.MinValue, float.MaxValue);
        //     _pitch += _input.PitchInputFixed * mouseYSpeed;
        //     _pitch = Functions.ClampAngle(_pitch, pitchMin, pitchMax);
        //     
        //     // 必须是 global rotation 才能解绑子物体的旋转和父物体
        //     cameraRoot.rotation = Quaternion.Euler(_pitch, _yaw, 0);
        // }
    }
}
