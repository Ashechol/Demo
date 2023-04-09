using Framework.Camera;
using Inputs;
using UnityEngine;

[RequireComponent(typeof(CameraHandler))]
public class Player : MonoBehaviour
{
    #region Components
    
    private CharacterController _controller;
    private InputHandler _input;
    private CameraHandler _camera;
    
    #endregion
    
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _velocityY;
    private Vector3 _rotation;

    #region Locomotion
    
    [Header("Movement")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float angularSpeed = 1000;

    [Header("Jump")] 
    public float gravity;
    
    #endregion
    
    private void Awake()
    {
        _controller = Functions.GetComponentSafe<CharacterController>(gameObject);
        _input = Functions.GetComponentSafe<InputHandler>(gameObject);
        _camera = Functions.GetComponentSafe<CameraHandler>(gameObject);
    }

    private void Update()
    {
        Locomotion();
    }

    private void Locomotion()
    {
        _direction = new Vector3(_input.MoveInputX, 0, _input.MoveInputY);
        if (_direction != Vector3.zero)
            _velocity = Quaternion.LookRotation(_direction) * _camera.cameraRoot.forward * runSpeed;
        else
            _velocity = Vector3.zero;

        if (!_controller.isGrounded)
            _velocityY -= gravity * Time.deltaTime;
        else
            _velocityY = -1f;
        _velocity.y = _velocityY;
        _controller.Move(_velocity * Time.deltaTime);
        Rotation();
    }

    private void Rotation()
    {
        if (_direction != Vector3.zero)
            _rotation = Vector3.RotateTowards(transform.forward, Quaternion.LookRotation(_direction) * _camera.cameraRoot.forward, 
                angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);

        transform.rotation = Quaternion.LookRotation(_rotation);
    }
}
