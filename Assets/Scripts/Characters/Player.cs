using System;
using Inputs;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components
    
    private CharacterController _controller;
    private InputHandler _input;
    
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
    }

    private void Update()
    {
        Locomotion();
    }

    private void Locomotion()
    {
        Rotation();
        
        _velocity = _direction * runSpeed;
        
        if (!_controller.isGrounded)
            _velocityY -= gravity * Time.deltaTime;
        else
            _velocityY = -1f;
        _velocity.y = _velocityY;
        
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Rotation()
    {
        _direction.Set(_input.MoveInputX, 0, _input.MoveInputY);
        _rotation = Vector3.RotateTowards(transform.forward, _direction, 
            angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);
        
        transform.rotation = Quaternion.LookRotation(_rotation);
    }
}
