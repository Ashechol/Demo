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
    public float angularSpeed = 800;

    [Header("Jump")] 
    public float gravity = 20;
    
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
        
    }

    private void Rotation()
    {
        
    }
}
