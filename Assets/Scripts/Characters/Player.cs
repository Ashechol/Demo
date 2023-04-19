using Framework.Camera;
using Inputs;
using UnityEngine;
using Utils;

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
        _controller = this.GetComponentSafe<CharacterController>();
        _input = this.GetComponentSafe<InputHandler>();
        _camera = this.GetComponentSafe<CameraHandler>();
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
